CREATE PROCEDURE [dbo].[spCreateBankAccount]
	@Balance real,
	@UserId int
AS
	declare @genBankAccNum bigint
	declare @genCardNum bigint
Begin
	select @genBankAccNum = Max(AccNum)+1  from BankAccounts
	select @genCardNum = cast(Max(CardNum)as bigint)  +1  from BankAccounts

	INSERT INTO [dbo].[BankAccounts]
		   (
			AccNum,
			Balance,
			CardNum,
			UserId
		   )
     VALUES
           (
			@genBankAccNum,
			@Balance,
			@genCardNum,
			@UserId
			)
End

