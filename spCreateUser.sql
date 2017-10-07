CREATE PROCEDURE [dbo].[spCreateUser]
	@FirstName nvarchar(max),
	@LastName nvarchar(max),
	@PidNum date,
	@PlaceOfBirth nvarchar(max),
	@Citizenship nvarchar(max),
    @Street nvarchar(max),
    @NumberOfDescriptive nvarchar(max),
    @Town nvarchar(max),
    @Zip nvarchar(max),
    @Email nvarchar(max),
    @Phone nvarchar(max),
    @MailStreet nvarchar(max),
    @MailNumberOfDescriptive nvarchar(max),
    @MailTown nvarchar(max),
    @MailZip nvarchar(max),
    @Password nvarchar(max),
    @Admin bit,
	@generatedUsername int output
	
AS
Begin
	select @generatedUsername = Max(UserName)+1  from UserAccounts where UserName not like 'Admin%' and UserName not like 'User%'

	INSERT INTO [dbo].[UserAccounts]
		   ([UserName]
		   ,[FirstName]
           ,[LastName]
           ,[PidNum]
           ,[PlaceOfBirth]
           ,[Citizenship]
           ,[Street]
           ,[NumberOfDescriptive]
           ,[Town]
           ,[Zip]
           ,[Email]
           ,[Phone]
           ,[MailStreet]
           ,[MailNumberOfDescriptive]
           ,[MailTown]
           ,[MailZip]
           ,[Password]
           ,[Admin])
     VALUES
           (
		   @generatedUsername,
           @FirstName,
           @LastName,
           @PidNum,
           @PlaceOfBirth,
           @Citizenship,
           @Street,
           @NumberOfDescriptive,
           @Town,
           @Zip,
           @Email,
           @Phone,
           @MailStreet,
           @MailNumberOfDescriptive,
           @MailTown,
           @MailZip,
           @Password,
           @Admin)
End

go
