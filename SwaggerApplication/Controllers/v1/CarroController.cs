using Microsoft.AspNetCore.Mvc;
using SwaggerApplication.Models;
using System.Collections.Generic;

namespace SwaggerApplication.Controllers
{
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CarroController : ControllerBase
    {
        private List<Carro> _carros;

        public CarroController()
        {
            _carros = new List<Carro>();

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
        /// Este método serve para obter a listagem de todos os carros cadastrados.
        /// </summary>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_carros);
        }

        /// <summary>
        /// Este método serve para obter um carro passando um ID por parametro.
        /// </summary>
        /// <param name="id">Este parâmetro é o ID do carro.</param>
        [HttpGet("{id}", Name = "Get" )]
        public IActionResult GetById(int id)
        {
            var carro = _carros.Find(c => c.Id == id);
            
            if(carro == null)
                return NotFound("Carro não encontrado na Listagem");

            return Ok(carro);
        }

        [MapToApiVersion("1.0")]
        [HttpPost]
        public IActionResult Post(Carro carro)
        {
            _carros.Add(carro);

            return CreatedAtAction("Get",new { id = carro.Id }, carro);
        }

        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Carro carro)
        {
            if (id != carro.Id || carro == null)
                return BadRequest();

            _carros[_carros.FindIndex(p => p.Id == id)] = carro;

            return NoContent();
        }

        [MapToApiVersion("2.0")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var carro = _carros.Find(c => c.Id == id);

            if (carro == null)
                return NotFound();

            _carros.Remove(carro);

            return Ok(carro);
        }


    }
}