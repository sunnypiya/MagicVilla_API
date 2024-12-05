using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_API.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(VillaStore.villsList);
        }
        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(200,Type = typeof(VillaDTO))]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villsList.FirstOrDefault(v => v.Id == id);
            if (villa == null)
            {
                NotFound();
            }
            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> CreateVill([FromBody] VillaDTO villa)
        {
            //if (!ModelState.IsValid)
            //{
            //    BadRequest(ModelState);
            //}
            if(VillaStore.villsList.FirstOrDefault(v => v.Name.ToLower() == villa.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa Name Already Exists!");
                return BadRequest(ModelState);
            }
            if (villa == null)
            {
                return BadRequest(villa);
            }
            if (villa.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            villa.Id = VillaStore.villsList.Max(v => v.Id) + 1;
            VillaStore.villsList.Add(villa);
            return CreatedAtRoute($"GetVilla", new { id = villa.Id }, villa);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteVilla(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villsList.FirstOrDefault(v => v.Id == id);
            if(villa == null)
            {
                return NotFound(villa);
            }

            VillaStore.villsList.Remove(villa);
            return NoContent();
        }
        [HttpPut("{id:int}", Name ="UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<VillaDTO> UpdateVilla(int id, [FromBody] VillaDTO villaDTO)
        {
            if (villaDTO == null || id != villaDTO?.Id) {
                return BadRequest(villaDTO);
            }
            var villa = VillaStore.villsList.FirstOrDefault(v => v.Id == id);

            if (villa == null) {
                return NotFound(villaDTO);
            }

            villa.Name = villaDTO.Name;
            villa.Occupancy = villaDTO.Occupancy;
            villa.Sqft = villaDTO.Sqft;

            return NoContent();
        }
        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<VillaDTO> UpdatePartialVilla(int id, [FromBody] JsonPatchDocument<VillaDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest(patchDTO);
            }
            var villa = VillaStore.villsList.FirstOrDefault(v => v.Id == id);

            if (villa == null)
            {
                return NotFound(patchDTO);
            }

            patchDTO.ApplyTo(villa,ModelState);
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            
            return NoContent();
        }
    }
}
