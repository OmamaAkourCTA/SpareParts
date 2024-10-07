using SparePartsModule.Infrastructure.ViewModels;
using System;

namespace SparePartsModule.Interface
{
	public interface IJWTManagerManager
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        TokenOutputDto GenerateToken(TokenInputDto model);
    }
}

