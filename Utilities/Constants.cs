using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartAppWS.Utilities
{
    public static class Constants
    {
        public static IConfiguration configuracion;
        public static readonly string ERROR = "Error";
        public static IActionResult InternalServerError()
        {
            return new StatusCodeResult(500);
        }
        public enum Status {
            [Description("Eliminado")]
            ELIMINADO = 0,
            [Description("Activo")]
            ACTIVO = 1,
            [Description("Inactivo")]
            INACTIVO = 2,
            [Description("Anulado")]
            ANULADO = 3,
            [Description("NO VALIDADO")]
            NO_VALIDADO = 4,
            [Description("password change required")]
            PASSWORD_CHANGE_REQUIRED = 5
        }


        public enum Respuestas
        {
            [Description("Succesful Transaction")]
            EXITOSO = 1,
            NO_AUTORIZADO = 401,
            INTERNAL_SERVER_ERROR = 500,
            NO_MODIFICADO = 304,
            METODO_PAGO_REQUERIDO = 402,
            BAD_REQUEST = 400,
            NETWORK_AUTENTICATION_REQUIRED = 511,
            NO_IMPLEMENTADO = 501
        }

        public enum Errors
        {
            [Description("Name can not be empty")]
            NAME_EMPTY,
            [Description("State name can not be empty")] 
            STATENAME_EMPTY,
            [Description("First name can not be empty")]
            FIRSTNAME_EMPTY,
            [Description("Last name can not be empty")]
            LASTNAME_EMPTY,
            [Description("Phone can not be empty")]
            PHONE_EMPTY,
            [Description("Email can not be empty")]
            EMAIL_EMPTY,
            [Description("Password can not be empty")]
            PASSWORD_EMPTY, 
            [Description("End date can not be less than Start date")]
            PERIOD_DATE,
            [Description("End time can not be less than Start time")]
            PERIOD_TIME,
            [Description("Price can no be less than 0")]
            PRICE_NEGATIVE,
            [Description("You must select at least one product")]
            NO_PRODUCTS,
            [Description("Category can no be empty")]
            CATEGORY_EMPTY, 
            [Description("Subcategory can no be empty")]
            SUBCATEGORY_EMPTY,
            [Description("Group can no be empty")]
            TOPPINGTYPE_EMPTY,
            [Description("User name can not be empty")]
            USERNAME_EMPTY,
            [Description("Credit card field is empty")]
            CREDITCARD_EMPTY,
            [Description("Credit card number is incorrect")]
            CREDITCARD_INCORRECT,
            [Description("Username and/or password is empty")]
            LOGIN_EMPTY,
            [Description("Username and/or password is incorrect or does not exist in our records")]
            LOGIN_INCORRECT,
            [Description("this Email exist in our records")]
            DUPLICATE_EMAIL,
            [Description("this Email was not typed correctly")]
            BAD_EMAIL_FORMAT,
            [Description("Address not found")]
            ADDRESS_NOT_FOUND,
            [Description("Category not found")]
            CATEGORY_NOT_FOUND,
            [Description("Category not found")]
            CITY_NOT_FOUND,
            [Description("Claim not found")]
            CLAIM_NOT_FOUND,
            [Description("Client not found")]
            CLIENT_NOT_FOUND,
            [Description("Event not found")]
            EVENT_NOT_FOUND,
            [Description("Offer not found")]
            OFFER_NOT_FOUND,
            [Description("Order not found")]
            ORDER_NOT_FOUND,
            [Description("Order type not found")]
            ORDERTYPE_NOT_FOUND,
            [Description("Payment method not found")]
            PAYMENTMETHOD_NOT_FOUND,
            [Description("Product not found")]
            PRODUCT_NOT_FOUND,
            [Description("State not found")]
            STATE_NOT_FOUND,
            [Description("Sub category not found")]
            SUBCATEGORY_NOT_FOUND,
            [Description("Topping not found")]
            TOPPING_NOT_FOUND,
            [Description("Topping type not found")]
            TOPPINGTYPE_NOT_FOUND,
            [Description("User not found")]
            USER_NOT_FOUND,
        }

        public enum ValidationType
        {
            EMAIL = 1,
            SMS = 2,
            
        }

        public enum Tracking
        {
            RECHAZADA = 0,
            PENDIENTE = 1,
            ACEPTADA = 2,
            EN_PROCESO = 3,
            ENVIADA = 4,
            ENTREGADO = 5,
        }

        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            if (e is Enum)
            {
                Type type = e.GetType();
                Array values = System.Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        var descriptionAttribute = memInfo[0]
                            .GetCustomAttributes(typeof(DescriptionAttribute), false)
                            .FirstOrDefault() as DescriptionAttribute;

                        if (descriptionAttribute != null)
                        {
                            return descriptionAttribute.Description;
                        }
                    }
                }
            }

            return null; // could also return string.Empty
        }
    }
}

