﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     Il codice è stato generato da uno strumento.
//     Versione runtime:2.0.50727.4016
//
//     Le modifiche apportate a questo file possono provocare un comportamento non corretto e andranno perse se
//     il codice viene rigenerato.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IndianaPark.PercorsiAvventura.Persistence.SqlServer
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[System.Data.Linq.Mapping.DatabaseAttribute(Name="indianapark")]
	public partial class BriefingsDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertBriefings(Briefings instance);
    partial void UpdateBriefings(Briefings instance);
    partial void DeleteBriefings(Briefings instance);
    partial void InsertTIpiBriefing(TIpiBriefing instance);
    partial void UpdateTIpiBriefing(TIpiBriefing instance);
    partial void DeleteTIpiBriefing(TIpiBriefing instance);
    #endregion
		
		public BriefingsDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public BriefingsDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public BriefingsDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public BriefingsDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Briefings> Briefings
		{
			get
			{
				return this.GetTable<Briefings>();
			}
		}
		
		public System.Data.Linq.Table<TIpiBriefing> TIpiBriefing
		{
			get
			{
				return this.GetTable<TIpiBriefing>();
			}
		}
	}
	
	[Table(Name="dbo.model_briefings")]
	public partial class Briefings : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private string _model_key;
		
		private string _tipo_briefing;
		
		private string _nome;
		
		private System.TimeSpan _durata;
		
		private int _posti_totali;
		
		private bool _ask_user;
		
		private System.TimeSpan _walk_time;
		
		private string _ctor_parameters;
		
		private EntityRef<TIpiBriefing> _model_tipi_briefing;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnModelKeyChanging(string value);
    partial void OnModelKeyChanged();
    partial void OnTipoBriefingChanging(string value);
    partial void OnTipoBriefingChanged();
    partial void OnNomeChanging(string value);
    partial void OnNomeChanged();
    partial void OnDurataChanging(System.TimeSpan value);
    partial void OnDurataChanged();
    partial void OnPostiTotaliChanging(int value);
    partial void OnPostiTotaliChanged();
    partial void OnAskUserChanging(bool value);
    partial void OnAskUserChanged();
    partial void OnWalkTimeChanging(System.TimeSpan value);
    partial void OnWalkTimeChanged();
    partial void OnCtorParametersChanging(string value);
    partial void OnCtorParametersChanged();
    #endregion
		
		public Briefings()
		{
			this._model_tipi_briefing = default(EntityRef<TIpiBriefing>);
			OnCreated();
		}
		
		[Column(Name="model_key", Storage="_model_key", DbType="VarChar(50) NOT NULL", CanBeNull=false, IsPrimaryKey=true)]
		public string ModelKey
		{
			get
			{
				return this._model_key;
			}
			set
			{
				if ((this._model_key != value))
				{
					this.OnModelKeyChanging(value);
					this.SendPropertyChanging();
					this._model_key = value;
					this.SendPropertyChanged("ModelKey");
					this.OnModelKeyChanged();
				}
			}
		}
		
		[Column(Name="tipo_briefing", Storage="_tipo_briefing", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		private string TipoBriefing
		{
			get
			{
				return this._tipo_briefing;
			}
			set
			{
				if ((this._tipo_briefing != value))
				{
					this.OnTipoBriefingChanging(value);
					this.SendPropertyChanging();
					this._tipo_briefing = value;
					this.SendPropertyChanged("TipoBriefing");
					this.OnTipoBriefingChanged();
				}
			}
		}
		
		[Column(Name="nome", Storage="_nome", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string Nome
		{
			get
			{
				return this._nome;
			}
			set
			{
				if ((this._nome != value))
				{
					this.OnNomeChanging(value);
					this.SendPropertyChanging();
					this._nome = value;
					this.SendPropertyChanged("Nome");
					this.OnNomeChanged();
				}
			}
		}
		
		[Column(Name="durata", Storage="_durata", DbType="bigint NOT NULL")]
		public System.TimeSpan Durata
		{
			get
			{
				return this._durata;
			}
			set
			{
				if ((this._durata != value))
				{
					this.OnDurataChanging(value);
					this.SendPropertyChanging();
					this._durata = value;
					this.SendPropertyChanged("Durata");
					this.OnDurataChanged();
				}
			}
		}
		
		[Column(Name="posti_totali", Storage="_posti_totali", DbType="Int NOT NULL")]
		public int PostiTotali
		{
			get
			{
				return this._posti_totali;
			}
			set
			{
				if ((this._posti_totali != value))
				{
					this.OnPostiTotaliChanging(value);
					this.SendPropertyChanging();
					this._posti_totali = value;
					this.SendPropertyChanged("PostiTotali");
					this.OnPostiTotaliChanged();
				}
			}
		}
		
		[Column(Name="ask_user", Storage="_ask_user", DbType="Bit NOT NULL")]
		public bool AskUser
		{
			get
			{
				return this._ask_user;
			}
			set
			{
				if ((this._ask_user != value))
				{
					this.OnAskUserChanging(value);
					this.SendPropertyChanging();
					this._ask_user = value;
					this.SendPropertyChanged("AskUser");
					this.OnAskUserChanged();
				}
			}
		}
		
		[Column(Name="walk_time", Storage="_walk_time", DbType="bigint NOT NULL")]
		public System.TimeSpan WalkTime
		{
			get
			{
				return this._walk_time;
			}
			set
			{
				if ((this._walk_time != value))
				{
					this.OnWalkTimeChanging(value);
					this.SendPropertyChanging();
					this._walk_time = value;
					this.SendPropertyChanged("WalkTime");
					this.OnWalkTimeChanged();
				}
			}
		}
		
		[Column(Name="ctor_parameters", Storage="_ctor_parameters", DbType="Text NOT NULL", CanBeNull=false, UpdateCheck=UpdateCheck.Never)]
		public string CtorParameters
		{
			get
			{
				return this._ctor_parameters;
			}
			set
			{
				if ((this._ctor_parameters != value))
				{
					this.OnCtorParametersChanging(value);
					this.SendPropertyChanging();
					this._ctor_parameters = value;
					this.SendPropertyChanged("CtorParameters");
					this.OnCtorParametersChanged();
				}
			}
		}
		
		[Association(Name="TIpiBriefing_Briefings", Storage="_model_tipi_briefing", ThisKey="TipoBriefing", OtherKey="Tipo", IsForeignKey=true)]
		public TIpiBriefing TIpiBriefing
		{
			get
			{
				return this._model_tipi_briefing.Entity;
			}
			set
			{
				TIpiBriefing previousValue = this._model_tipi_briefing.Entity;
				if (((previousValue != value) 
							|| (this._model_tipi_briefing.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._model_tipi_briefing.Entity = null;
						previousValue.Briefings.Remove(this);
					}
					this._model_tipi_briefing.Entity = value;
					if ((value != null))
					{
						value.Briefings.Add(this);
						this._tipo_briefing = value.Tipo;
					}
					else
					{
						this._tipo_briefing = default(string);
					}
					this.SendPropertyChanged("TIpiBriefing");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.model_tipi_briefing")]
	public partial class TIpiBriefing : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private string _tipo;
		
		private string _framework_type;
		
		private EntitySet<Briefings> _model_briefings;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnTipoChanging(string value);
    partial void OnTipoChanged();
    partial void OnFrameworkTypeChanging(string value);
    partial void OnFrameworkTypeChanged();
    #endregion
		
		public TIpiBriefing()
		{
			this._model_briefings = new EntitySet<Briefings>(new Action<Briefings>(this.attach_model_briefings), new Action<Briefings>(this.detach_model_briefings));
			OnCreated();
		}
		
		[Column(Name="tipo", Storage="_tipo", DbType="VarChar(50) NOT NULL", CanBeNull=false, IsPrimaryKey=true)]
		public string Tipo
		{
			get
			{
				return this._tipo;
			}
			set
			{
				if ((this._tipo != value))
				{
					this.OnTipoChanging(value);
					this.SendPropertyChanging();
					this._tipo = value;
					this.SendPropertyChanged("Tipo");
					this.OnTipoChanged();
				}
			}
		}
		
		[Column(Name="framework_type", Storage="_framework_type", DbType="VarChar(255) NOT NULL", CanBeNull=false)]
		public string FrameworkType
		{
			get
			{
				return this._framework_type;
			}
			set
			{
				if ((this._framework_type != value))
				{
					this.OnFrameworkTypeChanging(value);
					this.SendPropertyChanging();
					this._framework_type = value;
					this.SendPropertyChanged("FrameworkType");
					this.OnFrameworkTypeChanged();
				}
			}
		}
		
		[Association(Name="TIpiBriefing_Briefings", Storage="_model_briefings", ThisKey="Tipo", OtherKey="TipoBriefing")]
		public EntitySet<Briefings> Briefings
		{
			get
			{
				return this._model_briefings;
			}
			set
			{
				this._model_briefings.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_model_briefings(Briefings entity)
		{
			this.SendPropertyChanging();
			entity.TIpiBriefing = this;
		}
		
		private void detach_model_briefings(Briefings entity)
		{
			this.SendPropertyChanging();
			entity.TIpiBriefing = null;
		}
	}
}
#pragma warning restore 1591
