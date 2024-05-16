DROP TABLE IF EXISTS supermarket_sales;

CREATE TABLE supermarket_sales(
    invoice_id character varying(15) PRIMARY KEY NOT NULL,
    branch character varying(1),
    city character varying(20),
    customer_type character varying(20),
	gender character varying(6),
	product_category character varying(25),
	unit_price NUMERIC(7,2),
	no_of_products_purchased INT,
	total_price_of_goods_sold NUMERIC(8,2),
	datetime_of_purchase DATE,
	tax_5_percent NUMERIC(10,4),
	total_sales_after_taxes NUMERIC(15,4),
	payment_type character varying(15),
	gross_margin_percentage NUMERIC(12,9),
	gross_income NUMERIC(7,4),
	customer_rating NUMERIC(3,1)
);

CREATE SEQUENCE location_seq START 1;
CREATE TABLE supermarket_location
(
    location_id character varying(20) PRIMARY KEY DEFAULT CONCAT('LOC_',nextval('location_seq')),
    branch character varying(1),
    city character varying(20)
);

CREATE SEQUENCE product_seq START 1;
CREATE TABLE supermarket_product
(
    product_id character varying(20) PRIMARY KEY DEFAULT CONCAT('PROD_',nextval('product_seq')),
    product_category character varying(25),
    unit_price numeric(7,2)
);

CREATE SEQUENCE date_seq START 1;
CREATE TABLE supermarket_date
(
    date_id character varying(20) PRIMARY KEY DEFAULT CONCAT('DATE_',nextval('date_seq')),
    datetime_of_purchase DATE
);

CREATE SEQUENCE customer_seq START 1;
CREATE TABLE supermarket_customer
(
    customer_id character varying(20) PRIMARY KEY DEFAULT CONCAT('CUST_',nextval('customer_seq')),
    customer_type VARCHAR(20),
	gender VARCHAR(6)
);


-- DROP TABLE IF EXISTS public.sales_fact;

CREATE TABLE IF NOT EXISTS public.sales_fact
(
	
    sales_fact_id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    invoice_id character varying(15)  NOT NULL,
    location_id character varying(20) ,
    product_id character varying(20),
    date_id character varying(20),
    customer_id character varying(20),
    unit_price numeric(7,2),
    quantity_sold integer,
    total_price_of_goods_sold numeric(8,2),
    tax_5_percent numeric(10,2),
    total_sales_after_taxes numeric(10,2),
    payment_type character varying(15) ,
    CONSTRAINT sales_fact_pkey PRIMARY KEY (sales_fact_id),
    CONSTRAINT customer_fkey FOREIGN KEY (customer_id)
        REFERENCES public.supermarket_customer (customer_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT date_fkey FOREIGN KEY (date_id)
        REFERENCES public.supermarket_date (date_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT location_fkey FOREIGN KEY (location_id)
        REFERENCES public.supermarket_location (location_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT product_fkey FOREIGN KEY (product_id)
        REFERENCES public.supermarket_product (product_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)



