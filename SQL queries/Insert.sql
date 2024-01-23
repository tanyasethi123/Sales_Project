	
INSERT INTO public.sales_fact(
invoice_id, location_id, product_id, date_id, customer_id, unit_price, quantity_sold, 
total_price_of_goods_sold, tax_5_percent, total_sales_after_taxes, payment_type, gross_margin_percentage, 
	gross_income, customer_rating)
SELECT
ss.invoice_id,
sl.location_id,
sp.product_id,
sd.date_id,
sc.customer_id,
ss.unit_price, 
ss.quantity_sold, ss.total_price_of_goods_sold, ss.tax_5_percent, ss.total_sales_after_taxes, 
ss.payment_type, ss.gross_margin_percentage, ss.gross_income, ss.customer_rating
FROM supermarket_sales ss
JOIN supermarket_customer sc ON sc.customer_type= ss.customer_type AND sc.gender= ss.gender
JOIN supermarket_date sd ON sd.datetime_of_purchase= ss.datetime_of_purchase
JOIN supermarket_location sl ON sl.branch= ss.branch AND sl.city=ss.city
JOIN supermarket_product sp ON sp.product_category= ss.product_category 

--order by customer_rating desc
Truncate table sales_fact;
select * from sales_fact where invoice_id= '869-11-3082';
select * from supermarket_sales where invoice_id= '869-11-3082';
select * from supermarket_location where location_id= 'LOC_2';
select * from supermarket_product where product_id= 'PROD_1';
select * from supermarket_date where date_id= 'DATE_875';
select * from supermarket_customer where customer_id= 'CUST_1';