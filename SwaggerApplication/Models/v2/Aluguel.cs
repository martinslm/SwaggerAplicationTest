using System;

namespace SwaggerApplication.Models.v2
{
    public class Aluguel
    {
        public int Id { get; set; }
        public decimal Valor { get; set; }
        public string NomeCliente { get; set; }
        public DateTime DataAluguel { get; set; }
        public int DiasEmprestimo { get; set; }
        public int IdCarro { get; set; }
    }
}
