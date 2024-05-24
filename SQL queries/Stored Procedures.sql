CREATE OR REPLACE FUNCTION total_sales_and_rating_by_product_category()
RETURNS TABLE (
    product_category VARCHAR,
    total_sales NUMERIC,
    average_rating NUMERIC
)
LANGUAGE plpgsql
AS $$
BEGIN
 	RETURN QUERY
	SELECT
		sp.product_category,
		SUM(sf.total_sales_after_taxes) as total_sales,
		ROUND(AVG(sf.customer_rating),1) as average_rating
	FROM
		sales_fact sf
	JOIN
		supermarket_product sp
	ON
		sf.product_id = sp.product_id
	GROUP BY
		sp.product_category
	ORDER BY
		total_sales DESC;
	
END;
$$

select * from total_sales_and_rating_by_product_category()

CREATE OR REPLACE FUNCTION get_monthly_sales()
RETURNS TABLE (
    month_year text,
    total_sales NUMERIC
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT
        TO_CHAR(sd.datetime_of_purchase, 'YYYY-MM') AS month_year,
        SUM(sf.total_sales_after_taxes) AS total_sales
    FROM
        Sales_Fact sf
    JOIN
        supermarket_date sd ON sf.date_id = sd.date_id
    GROUP BY
        TO_CHAR(sd.datetime_of_purchase, 'YYYY-MM')
    ORDER BY
        month_year;
END;
$$ ;

select * from get_monthly_sales();
