using Microsoft.AspNetCore.Mvc;
using SwaggerApplication.Models;
using SwaggerApplication.Models.v2;
using System.Collections.Generic;
using System.Linq;

namespace SwaggerApplication.Controllers.v2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class GestaoController : ControllerBase
    {
        private readonly List<Aluguel> _carrosAlugados;
        private readonly List<Carro> _carros;

        public GestaoController()
        {
            _carros = new List<Carro>();
            _carrosAlugados = new List<Aluguel>();

            var carro = new Carro()
            {
                Id = 1,
                Nome = "Uno",
                Fabricante = "Fiat",
                Ano = 2012
            };

            _carros.Add(carro);

            carro = new Carro()
            {
                Id = 2,
                Nome = "Palio",
                Fabricante = "Fiat",
                Ano = 2009
            };

            _carros.Add(carro);
        }

        /// <summary>
        /// Serve para que o cliente possa alugar um carro. 
        /// </summary>
        /// <param name="dadosAluguel"></param>
        /// <returns></returns>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [MapToApiVersion("2.0")]
        [HttpPost]
        [Route("AlugarCarro")]
        public IActionResult AlugarCarro([FromBody]Aluguel dadosAluguel)
        {
            if (dadosAluguel == null ||
               dadosAluguel == new Aluguel())
                return BadRequest();

            var idAluguelJaOcupado = _carrosAlugados.Where(c => c.Id == dadosAluguel.Id).Any();
            var carroExiste = _carros.Where(c => c.Id == dadosAluguel.IdCarro).Any();

            if (!carroExiste || idAluguelJaOcupado)
                return BadRequest();

            _carrosAlugados.Add(dadosAluguel);

            return Ok();
        }

    }
}
