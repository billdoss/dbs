using Dapper;
using dbs.Context;
using dbs.Filter;
using dbs.Helpers;
using dbs.Models;
using dbs.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Configuration;
using System.Linq;

namespace dbs.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUriService _uriService;
        private readonly AccountContext _accountContext;
        private readonly IConfiguration _config;

        public AccountController(AccountContext accountContext, IUriService uriService, IConfiguration config)
        {
            _accountContext = accountContext;
            _uriService = uriService;
            _config = config;
        }

        // without paging module
        //GET: api/Account
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<AccountResponse>>> GetItems()
        //{
        //    return await _accountContext.Account.ToListAsync();
        //}

        // with paging module
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _accountContext.Account
               .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
               .Take(validFilter.PageSize)
               .ToListAsync();
            var totalRecords = await _accountContext.Account.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<AccountResponse>(pagedData, validFilter, totalRecords, _uriService, route);
            return Ok(pagedReponse);
        }

        // GET: api/Account/5
        [HttpGet("{acctNum}")]
        public async Task<ActionResult<AccountResponse>> GetItem(string acctNum)
        {
            //var item = await _accountContext.Account.FindAsync(acctNum);
            var item = _accountContext.Account.FirstOrDefault(u => u.account_number == acctNum);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        // POST: api/Account
        //[HttpPost]
        //public async Task<ActionResult<AccountResponse>> PostItem(Account account)
        //{
        //    AccountResponse accountResponse = new AccountResponse()
        //    {
        //        account_number = account.account_number,
        //        branch_name = account.account_number,
        //        label = account.account_number,
        //        account_type = account.account_number,
        //        status = account.account_number,
        //        card_number = account.account_number,
        //        address = account.account_number
        //    };
        //    _accountContext.Account.Add(accountResponse);
        //    await _accountContext.SaveChangesAsync();

        //    return CreatedAtAction(nameof(GetItem), new { id = accountResponse.account_number }, account);
        //}

        // POST: api/Account
        [HttpPost]
        public async Task<ActionResult<Account>> PostItem(AccountHandler account)
        {
            // create database if it doesn't exist
            var connectionString = _config.GetValue<string>("ConnectionStrings:DefaultConnection");
            using var connection = new NpgsqlConnection(connectionString);
            var sqlDbCount = $"SELECT generate_account('{account.branch_name}', '{account.label}', '{account.account_type}', '{account.status}', '{account.card_number}', '{account.address}');";
            var dbCount = connection.Query<string>(sqlDbCount);
            
            return new Account() {
                account_number = dbCount.ToList().ElementAt(0).ToString(),
                branch_name = account.branch_name,
                account_type = account.account_type,
                address = account.address,
                card_number = account.card_number,
                label = account.label,
                status = account.status                
            } ;
        }

        // PUT: api/Account/CIXXXXXXXXXXXXXXXXX
        [HttpPut("{acctNum}")]
        public async Task<IActionResult> PutItem(string acctNum, AccountHandler account)
        {
            var item = _accountContext.Account.FirstOrDefault(u => u.account_number == acctNum);
           
            if (item == null)
            {
                return NotFound();
            }

            AccountResponse accountResponse = new AccountResponse()
            {
                account_number = acctNum,
                branch_name = account.branch_name,
                account_type = account.account_type,
                address = account.address,
                card_number = account.card_number,
                label = account.label,
                status = account.status,
                updated_date = DateTime.UtcNow

            };

            _accountContext.Account.Update(accountResponse).State = EntityState.Modified;

            try
            {
                await _accountContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(acctNum))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Account/CI164XXXXXXXXXXXXXXXXX
        [HttpDelete("{acctNum}")]
        public async Task<IActionResult> DeleteItem(string acctNum)
        {
            var item = _accountContext.Account.FirstOrDefault(u => u.account_number == acctNum);

            if (item == null)
                if (!ItemExists(acctNum))
            {
                return NotFound();
            }

            _accountContext.Account.Remove(item);
            await _accountContext.SaveChangesAsync();

            return NoContent();
        }

        private bool ItemExists(string acctNum)
        {
            return _accountContext.Account.Any(e => e.account_number == acctNum);
        }
    }
}