using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using CoreBase.Interfaces;

namespace CoreBase
{
    [DataContract(IsReference=true)]
    public abstract class BaseModel : INotifyPropertyChanged, INotifyDataPropertyChanged, IChangeTracking
    {

        #region Constructors

        public BaseModel ()
        {
            CanNotify = false;
        }

        #endregion



        #region Properties

        BaseModel _parent;

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        [System.Runtime.Serialization.DataMember]
        public BaseModel Parent
        {
            get { return _parent;  }
            set
            {
                _parent = value;
            }
        }


        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public virtual string ItemFirstIdentifier
        {
            get { return string.Empty; }
        }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public virtual string ItemSecondIdentifier
        {
            get { return string.Empty; }
        }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public virtual string ItemThirdIdentifier
        {
            get { return string.Empty; }
        }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public virtual string ItemFourthIdentifier
        { get; }

        bool _isMinimum;

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        [System.Runtime.Serialization.DataMember]
        public bool IsMinimum
        {
            get { return _isMinimum; }
            set
            {
                _isMinimum = value;
            }
        }


        int _relationships;

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        [System.Runtime.Serialization.DataMember]
        public int Relationships
        {
            get { return _relationships; }
            set
            {
                _relationships = value;
            }
        }


        string _parentModelId;

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public virtual string ParentModelId
        {
            get { return _parentModelId; }
            set
            {
                _parentModelId = value;
            }
        }



        #endregion 

        #region Support Methods

        protected void SetField<T> (ref T field, T value, string propertyName)
        {
            if ( IgnoreChanges )
                return;

            if ( SkipCompare || !EqualityComparer<T>.Default.Equals( field, value ) )
            {
                T oldValue = field;
                T newValue = value;

                field = value;

                IsChanged = true;

                if ( Parent != null )
                    Parent.IsChanged = true;

                OnDataPropertyChanged<T>( propertyName, oldValue, newValue );
            }
        }

        #endregion


        #region ITrace

        bool _isTraceEnabled = false;

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public virtual bool IsTraceEnabled
        {
            get { return _isTraceEnabled; }
            set
            {
                _isTraceEnabled = value;
            }
        }



        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string TraceId
        {
            get { return this.GetType().Name; }
        }

        public void Trace ( string msg )
        {
            /*
            if ( IsTraceEnabled )
                LogProvider.Logger.LogInfo ( TraceId + " " + msg );
            else
                LogProvider.Logger.LogDebug ( TraceId + " " + msg );
            */
        }


        #endregion


        #region INotifyPropertyChanged Implementation


        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged ( string propertyName )
        {

            if ( !CanNotify )
            {
                Trace ( "BaseModel.OnPropertyChanged NOTIFY off " + propertyName );
                return;
            }

            var handler = PropertyChanged;
            if ( handler != null )
            {
                handler ( this, new PropertyChangedEventArgs ( propertyName ) );
                Trace ( "BaseModel.OnPropertyChanged property changed " + propertyName );
            }
            else
                Trace ( "BaseModel.OnPropertyChanged NO handler property " + propertyName );
        }

        #endregion


        #region INotifyDataPropertyChanged Implementation

        private object _changeLock = new object ();

        private List<string> _propertyChangeList = new List<string> ();


        public event DataPropertyChangedEventHandler DataPropertyChanged;

        public virtual void OnDataPropertyChanged<T> ( string propertyName, T oldValue, T newValue ) 
        {
            // this should only be hit one time
            // ObjectUtilities.Clone method bypasses the call to create _changeLock inline
            // so we create here
            if ( _changeLock == null )
            { 
                // we only have this to use to lock object 
                // right now
                lock ( this )
                {
                    _changeLock = new object ();
                    _propertyChangeList = new List<string> ();
                }                
            }




            if ( !CanNotify )
                return;

            var handler = DataPropertyChanged;
            if ( handler != null )
            {
                handler( this,
                    new DataPropertyChangedEventArgs()
                    {
                        Change =
                        new DataPropertyChanged()
                        {
                            ChangedObject = this,
                            PropertyName = propertyName,
                            Entity = this,
                            OldValue = oldValue,
                            NewValue = newValue
                        }
                    } );
            }

            OnPropertyChanged ( propertyName );

        }

        #endregion

        #region IChangeTracking Implementation

        public virtual void AcceptChanges ()
        {
        }

        public bool _isChanged = false;

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        [System.Runtime.Serialization.DataMember]
        public virtual bool IsChanged
        {
            get { return _isChanged; }
            set 
            {
                if ( Parent != null )
                    Parent._isChanged = value;

                _isChanged = value;
            }
        }

        private bool _canNotify = false;

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        [System.Runtime.Serialization.DataMember]
        public virtual bool CanNotify
        {
            get { return _canNotify; }
            set
            {
                if ( Parent != null )
                    Parent._canNotify = value;

                _canNotify = value;
            }
        }


        public bool _isNew = false;

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        [System.Runtime.Serialization.DataMember]
        public virtual bool IsNew
        {
            get { return _isNew; }
            set
            {
                if ( Parent != null )
                    Parent._isNew = value;

                _isNew = value;
            }
        }


        private bool _ignoreChanges = false;

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        protected bool IgnoreChanges
        {
            get { return _ignoreChanges; }
            set
            {
                _ignoreChanges = value;
            }
        }


        private bool _skipCompare = false;

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public bool SkipCompare
        {
            get { return _skipCompare; }
            set
            {
                _skipCompare = value;
            }
        }

        #endregion



        #region Support Methods


        public void Undo ( DataPropertyChanged change )
        {
            // dont notify on an undo
            CanNotify = false;

            // restore the old value
            PropertyInfo propertyInfo = GetType().GetProperty( change.PropertyName );
            if ( propertyInfo != null )
            {
                Trace( "SETTING " + change.PropertyName + " to " + change.OldValue );
                propertyInfo.SetValue( this, change.OldValue, null );
                Trace( "SET " + change.PropertyName + " to " + change.OldValue );
            }

            // re-enable notification
            CanNotify = true;

            // tell UI value changed
            OnPropertyChanged( change.PropertyName );

        }


        #endregion 
    }
}
