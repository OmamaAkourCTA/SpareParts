using MarkaziaPOS.API.Extensions;
using Microsoft.AspNetCore.Mvc;
using SparePartsModule.Infrastructure.ViewModels;
using SparePartsModule.Infrastructure.ViewModels.Models;
using SparePartsModule.Infrastructure.ViewModels.Models.ListNames;
using SparePartsModule.Interface;
using System.ComponentModel.DataAnnotations;

namespace SparePartsModule.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryListNamesController : ControllerBase
    {
        private readonly ILibraryListNamesService _service;
        private readonly ILogger<LibraryListNamesController>? _logger;

        public LibraryListNamesController(ILibraryListNamesService service, ILogger<LibraryListNamesController>? logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger;
        }

   



    }
}
