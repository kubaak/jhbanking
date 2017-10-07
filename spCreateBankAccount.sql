CREATE PROCEDURE [dbo].[spCreateBankAccount]
	@Balance real,
	@UserId int
AS
	declare @genBankAccNum int
Begin
	select @genBankAccNum = Max(AccNum)+1  from BankAccounts

	INSERT INTO [dbo].[BankAccounts]
		   (
			AccNum,
			Balance,
			UserId
		   )
     VALUES
           (
			@genBankAccNum,
			@Balance,
			@UserId
			)
End

go