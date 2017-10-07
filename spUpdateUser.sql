
CREATE PROCEDURE [dbo].[spUpdateUser]
	@userId int,
	@UserName nvarchar(max),
	@FirstName nvarchar(max),
	@LastName nvarchar(max),
	@PidNum nvarchar(max),
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
    @Admin bit
	
AS
Begin
UPDATE [dbo].[UserAccounts]
   SET [UserName] = @UserName
      ,[FirstName] = @FirstName
      ,[LastName] = @LastName 
      ,[PidNum] = @PidNum
      ,[PlaceOfBirth] = @PlaceOfBirth
      ,[Citizenship] = @Citizenship
      ,[Street] =  @Street
      ,[NumberOfDescriptive] = @NumberOfDescriptive
      ,[Town] = @Town
      ,[Zip] = @Zip
      ,[Email] = @Email
      ,[Phone] = @Phone
      ,[MailStreet] = @MailStreet
      ,[MailNumberOfDescriptive] = @MailNumberOfDescriptive
      ,[MailTown] = @MailTown
      ,[MailZip] = @MailZip
      ,[Password] = @Password
      ,[Admin] = @Admin
	   WHERE [UserId] = @userId

end

GO

