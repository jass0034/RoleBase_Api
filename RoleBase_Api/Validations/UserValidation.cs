using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using RoleBase_Api.Data;
using RoleBase_Api.Enums;
using RoleBase_Api.Models.DTOs;

namespace RoleBase_Api.Validations
{
    public class UserValidation
    {
        public static string ValidationUser(RegisterDTO registerDTO)
        {
            if(registerDTO.RoleId== 3)
            {
                return ("Invalid RoleId .");
            }

            if (!Enum.IsDefined(typeof(GenderType), registerDTO.Gender))
            {
                return ("Invalid Gender");
            }

            return ("Valid");
        }
    }
}
