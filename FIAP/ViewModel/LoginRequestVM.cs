using System.ComponentModel.DataAnnotations;

namespace FIAP.ViewModel
{
    public class LoginRequestVM
    {

        [Required(ErrorMessage ="Email e requerido")]

        public string EmailUsuario {  get; set; }

        [Required(ErrorMessage = "Senha e requerida")]

        public string Senha { get; set; }

        public LoginRequestVM()
        {
            
        }

        public LoginRequestVM(string emailUsuario, string senha)
        {
            EmailUsuario = EmailUsuario;
            Senha = Senha;
        }

    }
}
