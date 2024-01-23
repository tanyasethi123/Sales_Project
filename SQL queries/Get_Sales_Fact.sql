SELECT invoice_id, location_id, product_id, date_id, customer_id, unit_price, quantity_sold, total_price_of_goods_sold, tax_5_percent, total_sales_after_taxes, payment_type, gross_margin_percentage, gross_income, customer_rating
	FROM public.sales_fact;
	
select * from sales_fact where invoice_id ilike '111%'

Delete from sales_fact where invoice_id='111'
Delete from sales_fact where invoice_id='Test1'
Delete from sales_fact where invoice_id='123-456-789'

Select sf.invoice_id, 
sf.location_id, sl.branch, 
sf.product_id,sp.product_category, 
sf.date_id, sd.datetime_of_purchase,
sf.customer_id, sc.customer_type,sc.gender,
sf.unit_price, sf.quantity_sold, sf.total_price_of_goods_sold, sf.tax_5_percent, 
sf.total_sales_after_taxes, sf.payment_type
From sales_fact sf 
Join supermarket_location sl On sf.location_id= sl.location_id
Join supermarket_product sp On sf.product_id = sp.product_id
Join supermarket_date sd On sf.date_id = sd.date_id
Join supermarket_customer sc On sf.customer_id= sc.customer_id

select * from supermarket_date where datetime_of_purchase= '2019-02-06 10:22:00'

