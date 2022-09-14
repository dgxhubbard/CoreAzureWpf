using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.ServiceModel;



namespace CoreBase.Security
{



    [DataContract]
    public class SecurityToken
    {

        #region Constructors

        public SecurityToken ()
        {
            Id = Guid.NewGuid ();
        }




        #endregion


        #region Properties

        public Guid Id
        { get; set; }

        public string UserName
        { get; set; }

        public object Payload
        { get; set; }


        #endregion 
    }


    [DataContract]
    public class BypassToken : SecurityToken
    { }

}
