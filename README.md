# Part_1.Web

This application can return a list of possible customers who has an outstanding bill that matches an amount is keyed-in. Please run the <strong>Part_1_script.sql</strong> script that contains the MSSQL Server Database schema for this application which is attached in the project root folder. You also have to run the <strong>SeedCustomerData.sql</strong> script because this application does not support manual customer insertion.

To find possible payors first you have to create bill from the "Create Bill" section. You can either partially pay the created outstanding bill or mark the bill as paid. Once a bill is fully paid for it can be displayed in only "All Bills" Section.

You can easily find possible payor(s) who has an outstanding bill that matches the amount keyed-in in "Find Possible Payors" section.
