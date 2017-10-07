CREATE PROCEDURE [dbo].[spUpdateBankAccount]
	@AccId int,
	@Balance real
	
AS
Begin

	UPDATE [dbo].[BankAccounts]

			SET Balance = @Balance,
		where AccId = @AccId

End

