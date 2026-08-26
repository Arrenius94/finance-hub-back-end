namespace FinanceHub.Domain.Entities;

public class User : BaseEntity
{
    public User(string name, string secondName, DateTime dateBirth, string email, string password, decimal? wallet)
    {
        Name = name;
        SecondName =  secondName;
        Email = email;
        Password = password;
        DateBirth = dateBirth;
        Wallet = wallet;
        ConfirmEmail = false;
        
        Categories = new List<Category>();
    }
    
    public string Name { get; private set; }
    public string SecondName { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public DateTime DateBirth { get; private set; }
    public decimal? Wallet { get; private set; }
    public bool ConfirmEmail { get; private set; }
    public string? VerifcationCode { get;  set; }
    public DateTime? CodeExpiration { get; set; }
    
    public virtual ICollection<Category> Categories { get; private set; }
    
    public void IncreaseValue(decimal value)
    {
        Wallet += value;
    }

    public void DecreaseValue(decimal value)
    {
        Wallet -= value;
    }
    
    public void ChangePassword(string newPassword)
    {
        Password = newPassword;
    }

    public void ConfirmUserEmail()
    {
        ConfirmEmail = true;
        VerifcationCode  = null;
        CodeExpiration = null;
    }
}