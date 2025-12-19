using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace RentCarServer.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[EnableQuery] // odata nın çaılışmasını sağlıyor
public class ODataController : ControllerBase
{

    public static IEdmModel GetEdmModel()
    {
        var builder = new ODataConventionModelBuilder();

        builder.EnableLowerCamelCase();
        return builder.GetEdmModel();
    }

}
