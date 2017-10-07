CREATE PROCEDURE [dbo].[spUpdateBankAccount]
	@AccId int,
	@Balance real,
	@CardNum nvarchar(max)
	
AS
Begin

	UPDATE [dbo].[BankAccounts]
		SET Balance = @Balance,
			CardNum = @CardNum
		where AccId = @AccId

End

