PGDMP  	                    |           dbs    16.3    16.3     �           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            �           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            �           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            �           1262    16397    dbs    DATABASE     v   CREATE DATABASE dbs WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'French_France.1252';
    DROP DATABASE dbs;
                postgres    false            �            1255    16408 4   generate_account(text, text, text, text, text, text)    FUNCTION     K  CREATE FUNCTION public.generate_account(branchname text, label text, accounttype text, status text, cardnumber text, address text) RETURNS character varying
    LANGUAGE plpgsql
    AS $$
DECLARE
    accountNo VARCHAR(24);
BEGIN
    -- Générer un numéro de compte aléatoire
    --accountNo := LPAD((random() * 10000000000)::bigint::text, 19, '0');
    accountNo := 'CIXXX' || LPAD((random() * 10000000000)::bigint::text, 19, '0');

    -- Vérifier l'unicité du numéro de compte
    WHILE EXISTS (SELECT 1 FROM "Account" WHERE account_number = accountNo) LOOP
        --accountNo := LPAD((random() * 10000000000)::bigint::text, 19, '0');
        accountNo := 'CIXXX-' || LPAD((random() * 10000000000)::bigint::text, 19, '0');
    END LOOP;

    -- Insérer le nouveau compte dans la table
    INSERT INTO "Account" (account_number, branch_name, label, account_type, status, card_number, address, creation_date, updated_date) 
		VALUES (accountNo, branchname, label, accounttype, status, cardnumber, address, now(), now());

    -- Retourner le numéro de compte
    RETURN accountNo;
END;
$$;
 �   DROP FUNCTION public.generate_account(branchname text, label text, accounttype text, status text, cardnumber text, address text);
       public          postgres    false            �            1259    16409    Account    TABLE     1  CREATE TABLE public."Account" (
    account_number text NOT NULL,
    branch_name text NOT NULL,
    label text NOT NULL,
    account_type text NOT NULL,
    status text NOT NULL,
    card_number text NOT NULL,
    address text NOT NULL,
    creation_date date NOT NULL,
    updated_date date NOT NULL
);
    DROP TABLE public."Account";
       public         heap    postgres    false            �            1259    16399    users    TABLE     l   CREATE TABLE public.users (
    id text NOT NULL,
    username text NOT NULL,
    password text NOT NULL
);
    DROP TABLE public.users;
       public         heap    postgres    false            �          0    16409    Account 
   TABLE DATA           �   COPY public."Account" (account_number, branch_name, label, account_type, status, card_number, address, creation_date, updated_date) FROM stdin;
    public          postgres    false    216   �       �          0    16399    users 
   TABLE DATA           7   COPY public.users (id, username, password) FROM stdin;
    public          postgres    false    215          !           2606    16415    Account account_pkey 
   CONSTRAINT     `   ALTER TABLE ONLY public."Account"
    ADD CONSTRAINT account_pkey PRIMARY KEY (account_number);
 @   ALTER TABLE ONLY public."Account" DROP CONSTRAINT account_pkey;
       public            postgres    false    216                       2606    16405    users users_pkey 
   CONSTRAINT     N   ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (id);
 :   ALTER TABLE ONLY public.users DROP CONSTRAINT users_pkey;
       public            postgres    false    215            �   A  x����n�0���)x�NN���q�v�����B��*l��~�UŊ�64+��ﳌ��'�B�l˶�����[SlcSƼ�����i��.�粯��p)M��8p�Y9�[f��q`IiRJR��RR���s�w�uS\� 3�/��fss���#�ȭ�n}��j�a���*��h����{y�@��W�t~���T��9U�XUk��i������s-�s�B��6�Pt�8��AS�L�������m�0��,�E�o�.�n
y��L�b^��Q	<��`/�R#,���������{���,F�����3��)˲o�knu      �   i   x�s����0�sCsCCCs#N��ļ�CN�Ĥ�C.gu e��Ɯ����)�
ɩy%E�
����yɩ
)�
4�&�@����M��N�W� !/&     