# Sales Demo Project


This project aims to analyze and visualize sales data using various technologies and methodologies. Below is a summary of the project steps and components:

## Project Steps:
1. **Data Acquisition and Preparation:**
	Created a raw database from sample sales datasets.
	Utilized ETL (Extract, Transform, Load) processes with Pentaho Data Integration Software
	to load the raw data into the database.
2. **Data Modeling:**
	Designed a star schema Multidimensional Data Model (MDM) based on the raw database.

3. **Data Transformation and Loading:**
	Employed ELT (Extract, Load, Transform) processes to refresh the MDM from the raw data
	tables, ensuring the model remains up-to-date with the latest data.
4. **Web Application Development:**
	Developed a web application using ASP.NET Core C# to connect to the MDM schema.
	Implemented functionality to display the sales data in tabular format on a web page.
	Included features such as update, add, pagination, etc., to enhance user experience and 	data interaction.
5. **Stored Procedures:**
	Created stored procedures for specific analyzed data requirements.
	Integrated these stored procedures into the ASP.NET application to retrieve and display 	relevant data.

##Technologies Used:
1.	Pentaho Data Integration Software for ETL processes.
2.	ASP.NET Core C# for web application development.
3.	SQL Server for database management.
4.	HTML, CSS, and JavaScript for front-end development.
5.	Stored Procedures in SQL Server for data analysis and retrieval.
