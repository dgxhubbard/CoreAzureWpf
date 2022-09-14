using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using CoreBase.Enums;

namespace CoreBase.Interfaces
{

    [System.Runtime.Serialization.DataContractAttribute ( IsReference = true )]
    [System.Runtime.Serialization.KnownType ( typeof ( DataChangeType ) )]

    public class DataPropertyChanged
    {
        #region Constructors

        public DataPropertyChanged ()
        {
            Changed = DateTime.Now;
        }


        #endregion

        #region Properties


        /// <summary>
        /// Show if change can be undone
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public virtual bool EnableUndo
        { get; set; }

        /// <summary>
        /// Show what type of change occured
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public virtual DataChangeType ChangeType
        { get; set; }

        public virtual string UserName
        { get; set; }


        /// <summary>
        /// If the changed object has a parent it is stored here
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public virtual object ParentObject
        { get; set; }


        /// <summary>
        /// If the changed object has a parent its rid is stored here
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public virtual int ParentObjectRid
        { get; set; }



        /// <summary>
        /// If change type is PropertyChanged, this is the object where the change occured
        /// If change type is Delete this is the deleted object
        /// If change type is Insert this is the new object
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public virtual object ChangedObject
        { get; set; }

        /// <summary>
        /// This is the entity that had the property change
        /// Sometime a ChangeObject can be the entity (db object) or it 
        /// can be a model if the property exists on the model for design reasons.
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public virtual object Entity
        { get; set; }

        /// <summary>
        /// If change type is PropertyChanged, stores name of the property that changd
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public virtual string PropertyName
        { get; set; }

        /// <summary>
        /// If change type is PropertyChanged, this is the new property value.
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public virtual object NewValue
        { get; set; }

        /// <summary>
        /// If change type is PropertyChanged, this is the old property value.
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public virtual object OldValue
        { get; set; }


        [System.Runtime.Serialization.DataMember]
        public virtual DateTime Changed
        { get; set; }

        #endregion


        #region Override Methods


        public override string ToString ()
        {
            return "Object " + ChangedObject.GetType().Name +  " Change: " + PropertyName + " NewValue: " + NewValue + " OldValue: " + OldValue;

        }

        #endregion

    }


    public class DataPropertyChangedEventArgs
    {
        #region Constructors

        public DataPropertyChangedEventArgs () :
            base()
        {}

        /*
        public DataPropertyChangedEventArgs ( string propertyName ) :
            base()
        {
            PropertyName = propertyName;
        }
        */

        public DataPropertyChanged Change
        { get; set; }





        #endregion

    }

    public delegate void DataPropertyChangedEventHandler ( object sender, DataPropertyChangedEventArgs e );

    public interface INotifyDataPropertyChanged
    {

        event DataPropertyChangedEventHandler DataPropertyChanged;
    }
}
