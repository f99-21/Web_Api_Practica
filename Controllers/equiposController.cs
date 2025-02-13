using System.Runtime.ConstrainedExecution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_Api_Practica.Models;

namespace Web_Api_Practica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class equiposController : ControllerBase
    {

        private readonly equiposContext _equiposContexto;

        public equiposController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }
        /// <summary>
        /// EndPoint que retorna el listado de todos los equipos existentes.
        /// </summary>
        /// <returns>Listado de equipos.</returns>
       [HttpGet("GetAll")]
        public IActionResult Get()
        {
            List<equipos> listadoEquipo = (from e in _equiposContexto.equipos
                                           select e).ToList();

            if (listadoEquipo.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoEquipo);
        }

        [HttpGet]
        [Route("GetById&{id}")]
        public IActionResult Get(int id)
        {

            equipos? equipo = (from e in _equiposContexto.equipos
                               where e.id_equipos == id
                               select e).FirstOrDefault();
            if (equipo == null)
            {
                return NotFound();
            }
            return Ok(equipo);

        }

        [HttpGet]
        [Route("Find/{filtro}")]
        public IActionResult FindByDescription(string filtro
            )
        {
            equipos? equipo = (from e in _equiposContexto.equipos
                               where e.descripcion.Contains(filtro)
                               select e).FirstOrDefault();
            if (equipo == null)
            {
                return NotFound();

            }
            return Ok(equipo);
        }

        [HttpPost]
        [Route("Add")]

        public IActionResult GuardarEquipo([FromBody] equipos equipo)
        {
            try
            {
                _equiposContexto.equipos.Add(equipo);
                _equiposContexto.SaveChanges();
                return Ok(equipo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarEquipo(int id, [FromBody] equipos equipoModificado)
        {
            equipos? equipoActual = (from e in _equiposContexto.equipos
                                     where e.id_equipos == id
                                     select e).FirstOrDefault();

            if (equipoActual == null)
            {
                return NotFound();
            }
            equipoActual.nombre = equipoModificado.nombre;
            equipoActual.descripcion = equipoModificado.descripcion;
            equipoActual.marca_id = equipoModificado.marca_id;
            equipoActual.tipo_equipo_id = equipoModificado.tipo_equipo_id;
            equipoActual.anio_compra = equipoModificado.anio_compra;
            equipoActual.costo = equipoModificado.costo;

            _equiposContexto.Entry(equipoActual).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _equiposContexto.SaveChanges();
            return Ok(equipoModificado);
        }

        [HttpDelete]
        [Route("eliminae/{id}")]

        public IActionResult EliminarEquipo(int id)
        {
            equipos? equipo = (from e in _equiposContexto.equipos
                               where e.id_equipos == id 
                               select e).FirstOrDefault();
            if (equipo == null)
                return NotFound();

            _equiposContexto.equipos.Attach(equipo);
            _equiposContexto.equipos.Remove(equipo);
            _equiposContexto.SaveChanges();

            return Ok(equipo);
        }

        [HttpGet]
        [Route("ver")]

        public ActionResult ver()
        {
            List<equipos> ListadoEquipo = (from e in _equiposContexto.equipos
                                           join t in _equiposContexto.tipo_equipo
                                           on e.tipo_equipo_id equals t.id_tipo_equipo
                                           join m in _equiposContexto.marcas
                                           on e.marca_id equals m.id_marcas
                                           join es in _equiposContexto.estados_equipo
                                           on e.estado_equipo_id equals es.id_estados_equipo
                                           select e).ToList();

            if (ListadoEquipo.Count == 0)
            {
                return NotFound();
            }
            return Ok(ListadoEquipo);
                                           
        }



        [HttpGet]
        [Route("informacion")]

        public ActionResult informacion()
        {
            var ListadoEquipo = (from e in _equiposContexto.equipos
                                           join t in _equiposContexto.tipo_equipo
                                           on e.tipo_equipo_id equals t.id_tipo_equipo
                                           join m in _equiposContexto.marcas
                                           on e.marca_id equals m.id_marcas
                                           join es in _equiposContexto.estados_equipo
                                           on e.estado_equipo_id equals es.id_estados_equipo
                                           select  new
                                           {
                                               e.id_equipos,
                                               e.nombre,
                                               e.descripcion,
                                               e.tipo_equipo_id,
                                               tipo_equipo = t.descripcion,
                                               e.marca_id,
                                               marcas = m.nombre_marca,
                                               e.estado_equipo_id,
                                               estados_equipo = es.descripcion,
                                               e.estado
                                           }).ToList();

            if (ListadoEquipo.Count == 0)
            {
                return NotFound();
            }
            return Ok(ListadoEquipo);

        }


        //En los modelos anónimos podemos retornar campos compuestos
        [HttpGet]
        [Route("inf")]

        public ActionResult inf()
        {
            var ListadoEquipo = (from e in _equiposContexto.equipos
                                 join t in _equiposContexto.tipo_equipo
                                 on e.tipo_equipo_id equals t.id_tipo_equipo
                                 join m in _equiposContexto.marcas
                                 on e.marca_id equals m.id_marcas
                                 join es in _equiposContexto.estados_equipo
                                 on e.estado_equipo_id equals es.id_estados_equipo
                                 select new
                                 {
                                     e.id_equipos,
                                     e.nombre,
                                     e.descripcion,
                                     e.tipo_equipo_id,
                                     tipo_equipo = t.descripcion,
                                     e.marca_id,
                                     marcas = m.nombre_marca,
                                     e.estado_equipo_id,
                                     estados_equipo = es.descripcion,
                                     detalle = $"Tipo : {t.descripcion}, Marca{m.nombre_marca},EstDO Equipo {es.descripcion}",
                                     e.estado
                                 }).ToList();

            if (ListadoEquipo.Count == 0)
            {
                return NotFound();
            }
            return Ok(ListadoEquipo);

        }


        //En ciertas ocasiones es necesario hacer consultas utilizando el TOP # para obtener un numero especifico de 
        //filas de la consulta, por ejemplo, obtener el top 10 de productos mas vendidos, el top 5 de usuarios que más
        [HttpGet]
        [Route("optener")]

        public ActionResult optener()
        {
            var ListadoEquipo = (from e in _equiposContexto.equipos
                                 join t in _equiposContexto.tipo_equipo
                                 on e.tipo_equipo_id equals t.id_tipo_equipo
                                 join m in _equiposContexto.marcas
                                 on e.marca_id equals m.id_marcas
                                 join es in _equiposContexto.estados_equipo
                                 on e.estado_equipo_id equals es.id_estados_equipo
                                 select new
                                 {
                                     e.id_equipos,
                                     e.nombre,
                                     e.descripcion,
                                     e.tipo_equipo_id,
                                     tipo_equipo = t.descripcion,
                                     e.marca_id,
                                     marcas = m.nombre_marca,
                                     e.estado_equipo_id,
                                     estados_equipo = es.descripcion,
                                     detalle = $"Tipo : {t.descripcion}, Marca{m.nombre_marca},EstDO Equipo {es.descripcion}",
                                     e.estado
                                 }).Take(1).ToList();   

            if (ListadoEquipo.Count == 0)
            {
                return NotFound();
            }
            return Ok(ListadoEquipo);

        }

        //El caso más común que se utiliza para saltar un grupo de registros de una consulta es cuando deseamos 
        //hacer un paginado de datos, por ejemplo, en la página 2, deseamos mostrar el segundo grupo de 10 registros

                [HttpGet]
        [Route("SaltarRegistro")]

        public ActionResult SaltarRegistro()
        {
            var ListadoEquipo = (from e in _equiposContexto.equipos
                                 join t in _equiposContexto.tipo_equipo
                                 on e.tipo_equipo_id equals t.id_tipo_equipo
                                 join m in _equiposContexto.marcas
                                 on e.marca_id equals m.id_marcas
                                 join es in _equiposContexto.estados_equipo
                                 on e.estado_equipo_id equals es.id_estados_equipo
                                 select new
                                 {
                                     e.id_equipos,
                                     e.nombre,
                                     e.descripcion,
                                     e.tipo_equipo_id,
                                     tipo_equipo = t.descripcion,
                                     e.marca_id,
                                     marcas = m.nombre_marca,
                                     e.estado_equipo_id,
                                     estados_equipo = es.descripcion,
                                     detalle = $"Tipo : {t.descripcion}, Marca{m.nombre_marca},EstDO Equipo {es.descripcion}",
                                     e.estado
                                 }).Skip(10).Take(10).ToList();

            if (ListadoEquipo.Count == 0)
            {
                return NotFound();
            }
            return Ok(ListadoEquipo);

        }
    }
    


}
