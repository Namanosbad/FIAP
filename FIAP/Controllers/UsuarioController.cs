using AutoMapper;
using FIAP.Models;
using FIAP.Repository.Interfaces;
using FIAP.ViewModel;
using Microsoft.AspNetCore.Mvc;
using AuthenticationService = FIAP.Services.AuthenticationService;

namespace FIAP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;

        private readonly IMapper _mapper;

        public UsuarioController(IUsuarioRepository usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IList<UsuarioResponseVM>>> GetAllAsync()
        {
            var usuarios = await _usuarioRepository.FindAllAsync();
            if (usuarios != null && usuarios.Count > 0)
            {
                return Ok(usuarios);
            }
            else
            {
                return NoContent();
            }
        }

        [HttpGet("{id}")]
        public async Task <ActionResult<UsuarioModel>> GetByIdAsync(int id)
        {
            var usuario = await _usuarioRepository.FindByIdAsync(id);
            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }


        [HttpPost]
        public async Task <ActionResult<UsuarioModel>> Post([FromBody] UsuarioModel usuarioModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuarioId = await _usuarioRepository.InsertAsync(usuarioModel);
            usuarioModel.UsuarioId = usuarioId;

            return CreatedAtAction(nameof(GetByIdAsync), new { id = usuarioId }, usuarioModel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] UsuarioModel usuarioModel)
        {
            if (id != usuarioModel.UsuarioId)
                return BadRequest("ID da URL diferente do corpo da requisição.");

            await _usuarioRepository.UpdateAsync(usuarioModel);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task <IActionResult> DeleteAsync(int id)
        {
            var usuario = await _usuarioRepository.FindByIdAsync(id);
            if (usuario == null)
                return NotFound();

           await _usuarioRepository.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost]
        [Route("Login")]
        public async Task <ActionResult<LoginResponseVM>> Login([FromBody] LoginRequestVM loginRequest)
        {
            if (ModelState.IsValid)
            {
                var usuarioModel = await _usuarioRepository.FindByEmailAndSenhaAsync(loginRequest.EmailUsuario, loginRequest.Senha);
                if (usuarioModel != null)
                {

                    var loginResponse = _mapper.Map<LoginResponseVM>(usuarioModel);
                    loginResponse.Token = AuthenticationService.GetToken(usuarioModel);

                    return Ok(loginResponse);
                }
                else
                {
                    return Unauthorized();
                }
            }else {
                var errors = ModelState.Values
                            .SelectMany(x => x.Errors)
                            .Select(m => m.ErrorMessage);

                return BadRequest(errors);
            }
        }
    }
}
