﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
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
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="indianapark")]
	public partial class ClientiDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertTableClienti(TableClienti instance);
    partial void UpdateTableClienti(TableClienti instance);
    partial void DeleteTableClienti(TableClienti instance);
    partial void InsertTableNominativi(TableNominativi instance);
    partial void UpdateTableNominativi(TableNominativi instance);
    partial void DeleteTableNominativi(TableNominativi instance);
    partial void InsertTableInserimenti(TableInserimenti instance);
    partial void UpdateTableInserimenti(TableInserimenti instance);
    partial void DeleteTableInserimenti(TableInserimenti instance);
    #endregion
		
		public ClientiDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ClientiDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ClientiDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ClientiDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<TableClienti> TableClienti
		{
			get
			{
				return this.GetTable<TableClienti>();
			}
		}
		
		public System.Data.Linq.Table<TableNominativi> TableNominativi
		{
			get
			{
				return this.GetTable<TableNominativi>();
			}
		}
		
		public System.Data.Linq.Table<TableInserimenti> TableInserimenti
		{
			get
			{
				return this.GetTable<TableInserimenti>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="model_clienti")]
	public partial class TableClienti : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private int _CodInserimento;
		
		private int _Codice;
		
		private string _TipoCliente;
		
		private System.Nullable<System.DateTime> _OraIngresso;
		
		private System.Nullable<System.DateTime> _OraUscita;
		
		private string _PrezzoBase;
		
		private string _Sconto;
		
		private string _ScontoComitiva;
		
		private bool _Uscito;
		
		private System.Nullable<decimal> _PrezzoFinale;
		
		private EntityRef<TableInserimenti> _TableInserimenti;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnCodInserimentoChanging(int value);
    partial void OnCodInserimentoChanged();
    partial void OnCodiceChanging(int value);
    partial void OnCodiceChanged();
    partial void OnTipoClienteChanging(string value);
    partial void OnTipoClienteChanged();
    partial void OnOraIngressoChanging(System.Nullable<System.DateTime> value);
    partial void OnOraIngressoChanged();
    partial void OnOraUscitaChanging(System.Nullable<System.DateTime> value);
    partial void OnOraUscitaChanged();
    partial void OnPrezzoBaseChanging(string value);
    partial void OnPrezzoBaseChanged();
    partial void OnScontoChanging(string value);
    partial void OnScontoChanged();
    partial void OnScontoComitivaChanging(string value);
    partial void OnScontoComitivaChanged();
    partial void OnUscitoChanging(bool value);
    partial void OnUscitoChanged();
    partial void OnPrezzoFinaleChanging(System.Nullable<decimal> value);
    partial void OnPrezzoFinaleChanged();
    #endregion
		
		public TableClienti()
		{
			this._TableInserimenti = default(EntityRef<TableInserimenti>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="id", Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="cod_inserimento", Storage="_CodInserimento", DbType="Int NOT NULL")]
		private int CodInserimento
		{
			get
			{
				return this._CodInserimento;
			}
			set
			{
				if ((this._CodInserimento != value))
				{
					this.OnCodInserimentoChanging(value);
					this.SendPropertyChanging();
					this._CodInserimento = value;
					this.SendPropertyChanged("CodInserimento");
					this.OnCodInserimentoChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="codice", Storage="_Codice", DbType="Int NOT NULL")]
		public int Codice
		{
			get
			{
				return this._Codice;
			}
			set
			{
				if ((this._Codice != value))
				{
					this.OnCodiceChanging(value);
					this.SendPropertyChanging();
					this._Codice = value;
					this.SendPropertyChanged("Codice");
					this.OnCodiceChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="tipo_cliente", Storage="_TipoCliente", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string TipoCliente
		{
			get
			{
				return this._TipoCliente;
			}
			set
			{
				if ((this._TipoCliente != value))
				{
					this.OnTipoClienteChanging(value);
					this.SendPropertyChanging();
					this._TipoCliente = value;
					this.SendPropertyChanged("TipoCliente");
					this.OnTipoClienteChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="ora_ingresso", Storage="_OraIngresso", DbType="DateTime")]
		public System.Nullable<System.DateTime> OraIngresso
		{
			get
			{
				return this._OraIngresso;
			}
			set
			{
				if ((this._OraIngresso != value))
				{
					this.OnOraIngressoChanging(value);
					this.SendPropertyChanging();
					this._OraIngresso = value;
					this.SendPropertyChanged("OraIngresso");
					this.OnOraIngressoChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="ora_uscita", Storage="_OraUscita", DbType="DateTime")]
		public System.Nullable<System.DateTime> OraUscita
		{
			get
			{
				return this._OraUscita;
			}
			set
			{
				if ((this._OraUscita != value))
				{
					this.OnOraUscitaChanging(value);
					this.SendPropertyChanging();
					this._OraUscita = value;
					this.SendPropertyChanged("OraUscita");
					this.OnOraUscitaChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="prezzo_base", Storage="_PrezzoBase", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string PrezzoBase
		{
			get
			{
				return this._PrezzoBase;
			}
			set
			{
				if ((this._PrezzoBase != value))
				{
					this.OnPrezzoBaseChanging(value);
					this.SendPropertyChanging();
					this._PrezzoBase = value;
					this.SendPropertyChanged("PrezzoBase");
					this.OnPrezzoBaseChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="sconto", Storage="_Sconto", DbType="VarChar(50)")]
		public string Sconto
		{
			get
			{
				return this._Sconto;
			}
			set
			{
				if ((this._Sconto != value))
				{
					this.OnScontoChanging(value);
					this.SendPropertyChanging();
					this._Sconto = value;
					this.SendPropertyChanged("Sconto");
					this.OnScontoChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="sconto_comitiva", Storage="_ScontoComitiva", DbType="VarChar(50)")]
		public string ScontoComitiva
		{
			get
			{
				return this._ScontoComitiva;
			}
			set
			{
				if ((this._ScontoComitiva != value))
				{
					this.OnScontoComitivaChanging(value);
					this.SendPropertyChanging();
					this._ScontoComitiva = value;
					this.SendPropertyChanged("ScontoComitiva");
					this.OnScontoComitivaChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="uscito", Storage="_Uscito", DbType="Bit NOT NULL")]
		public bool Uscito
		{
			get
			{
				return this._Uscito;
			}
			set
			{
				if ((this._Uscito != value))
				{
					this.OnUscitoChanging(value);
					this.SendPropertyChanging();
					this._Uscito = value;
					this.SendPropertyChanged("Uscito");
					this.OnUscitoChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="prezzo_finale", Storage="_PrezzoFinale", DbType="Decimal(18,2)")]
		public System.Nullable<decimal> PrezzoFinale
		{
			get
			{
				return this._PrezzoFinale;
			}
			set
			{
				if ((this._PrezzoFinale != value))
				{
					this.OnPrezzoFinaleChanging(value);
					this.SendPropertyChanging();
					this._PrezzoFinale = value;
					this.SendPropertyChanged("PrezzoFinale");
					this.OnPrezzoFinaleChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="TableInserimenti_TableClienti", Storage="_TableInserimenti", ThisKey="CodInserimento", OtherKey="Id", IsForeignKey=true)]
		public TableInserimenti TableInserimenti
		{
			get
			{
				return this._TableInserimenti.Entity;
			}
			set
			{
				TableInserimenti previousValue = this._TableInserimenti.Entity;
				if (((previousValue != value) 
							|| (this._TableInserimenti.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._TableInserimenti.Entity = null;
						previousValue.TableClienti.Remove(this);
					}
					this._TableInserimenti.Entity = value;
					if ((value != null))
					{
						value.TableClienti.Add(this);
						this._CodInserimento = value.Id;
					}
					else
					{
						this._CodInserimento = default(int);
					}
					this.SendPropertyChanged("TableInserimenti");
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
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="model_nominativi")]
	public partial class TableNominativi : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private int _Codice;
		
		private string _Nome;
		
		private EntitySet<TableInserimenti> _TableInserimenti;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnCodiceChanging(int value);
    partial void OnCodiceChanged();
    partial void OnNomeChanging(string value);
    partial void OnNomeChanged();
    #endregion
		
		public TableNominativi()
		{
			this._TableInserimenti = new EntitySet<TableInserimenti>(new Action<TableInserimenti>(this.attach_TableInserimenti), new Action<TableInserimenti>(this.detach_TableInserimenti));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="id", Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="codice", Storage="_Codice", DbType="Int NOT NULL")]
		public int Codice
		{
			get
			{
				return this._Codice;
			}
			set
			{
				if ((this._Codice != value))
				{
					this.OnCodiceChanging(value);
					this.SendPropertyChanging();
					this._Codice = value;
					this.SendPropertyChanged("Codice");
					this.OnCodiceChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="nome", Storage="_Nome", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string Nome
		{
			get
			{
				return this._Nome;
			}
			set
			{
				if ((this._Nome != value))
				{
					this.OnNomeChanging(value);
					this.SendPropertyChanging();
					this._Nome = value;
					this.SendPropertyChanged("Nome");
					this.OnNomeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="TableNominativi_TableInserimenti", Storage="_TableInserimenti", ThisKey="Id", OtherKey="CodNominativo")]
		public EntitySet<TableInserimenti> TableInserimenti
		{
			get
			{
				return this._TableInserimenti;
			}
			set
			{
				this._TableInserimenti.Assign(value);
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
		
		private void attach_TableInserimenti(TableInserimenti entity)
		{
			this.SendPropertyChanging();
			entity.TableNominativi = this;
		}
		
		private void detach_TableInserimenti(TableInserimenti entity)
		{
			this.SendPropertyChanging();
			entity.TableNominativi = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="model_inserimenti")]
	public partial class TableInserimenti : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private int _CodNominativo;
		
		private System.DateTime _DataInserimento;
		
		private string _ScontoComitiva;
		
		private System.Nullable<decimal> _PrezzoTotale;
		
		private string _TipoBiglietto;
		
		private EntitySet<TableClienti> _TableClienti;
		
		private EntityRef<TableNominativi> _TableNominativi;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnCodNominativoChanging(int value);
    partial void OnCodNominativoChanged();
    partial void OnDataInserimentoChanging(System.DateTime value);
    partial void OnDataInserimentoChanged();
    partial void OnScontoComitivaChanging(string value);
    partial void OnScontoComitivaChanged();
    partial void OnPrezzoTotaleChanging(System.Nullable<decimal> value);
    partial void OnPrezzoTotaleChanged();
    partial void OnTipoBigliettoChanging(string value);
    partial void OnTipoBigliettoChanged();
    #endregion
		
		public TableInserimenti()
		{
			this._TableClienti = new EntitySet<TableClienti>(new Action<TableClienti>(this.attach_TableClienti), new Action<TableClienti>(this.detach_TableClienti));
			this._TableNominativi = default(EntityRef<TableNominativi>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="id", Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="cod_nominativo", Storage="_CodNominativo", DbType="Int NOT NULL")]
		private int CodNominativo
		{
			get
			{
				return this._CodNominativo;
			}
			set
			{
				if ((this._CodNominativo != value))
				{
					this.OnCodNominativoChanging(value);
					this.SendPropertyChanging();
					this._CodNominativo = value;
					this.SendPropertyChanged("CodNominativo");
					this.OnCodNominativoChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="data_inserimento", Storage="_DataInserimento", DbType="DateTime NOT NULL")]
		public System.DateTime DataInserimento
		{
			get
			{
				return this._DataInserimento;
			}
			set
			{
				if ((this._DataInserimento != value))
				{
					this.OnDataInserimentoChanging(value);
					this.SendPropertyChanging();
					this._DataInserimento = value;
					this.SendPropertyChanged("DataInserimento");
					this.OnDataInserimentoChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="sconto_comitiva", Storage="_ScontoComitiva", DbType="VarChar(50)")]
		public string ScontoComitiva
		{
			get
			{
				return this._ScontoComitiva;
			}
			set
			{
				if ((this._ScontoComitiva != value))
				{
					this.OnScontoComitivaChanging(value);
					this.SendPropertyChanging();
					this._ScontoComitiva = value;
					this.SendPropertyChanged("ScontoComitiva");
					this.OnScontoComitivaChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="prezzo_totale", Storage="_PrezzoTotale", DbType="Decimal(18,2)")]
		public System.Nullable<decimal> PrezzoTotale
		{
			get
			{
				return this._PrezzoTotale;
			}
			set
			{
				if ((this._PrezzoTotale != value))
				{
					this.OnPrezzoTotaleChanging(value);
					this.SendPropertyChanging();
					this._PrezzoTotale = value;
					this.SendPropertyChanged("PrezzoTotale");
					this.OnPrezzoTotaleChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="tipo_biglietto", Storage="_TipoBiglietto", DbType="VarChar(50)")]
		public string TipoBiglietto
		{
			get
			{
				return this._TipoBiglietto;
			}
			set
			{
				if ((this._TipoBiglietto != value))
				{
					this.OnTipoBigliettoChanging(value);
					this.SendPropertyChanging();
					this._TipoBiglietto = value;
					this.SendPropertyChanged("TipoBiglietto");
					this.OnTipoBigliettoChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="TableInserimenti_TableClienti", Storage="_TableClienti", ThisKey="Id", OtherKey="CodInserimento")]
		public EntitySet<TableClienti> TableClienti
		{
			get
			{
				return this._TableClienti;
			}
			set
			{
				this._TableClienti.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="TableNominativi_TableInserimenti", Storage="_TableNominativi", ThisKey="CodNominativo", OtherKey="Id", IsForeignKey=true)]
		public TableNominativi TableNominativi
		{
			get
			{
				return this._TableNominativi.Entity;
			}
			set
			{
				TableNominativi previousValue = this._TableNominativi.Entity;
				if (((previousValue != value) 
							|| (this._TableNominativi.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._TableNominativi.Entity = null;
						previousValue.TableInserimenti.Remove(this);
					}
					this._TableNominativi.Entity = value;
					if ((value != null))
					{
						value.TableInserimenti.Add(this);
						this._CodNominativo = value.Id;
					}
					else
					{
						this._CodNominativo = default(int);
					}
					this.SendPropertyChanged("TableNominativi");
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
		
		private void attach_TableClienti(TableClienti entity)
		{
			this.SendPropertyChanging();
			entity.TableInserimenti = this;
		}
		
		private void detach_TableClienti(TableClienti entity)
		{
			this.SendPropertyChanging();
			entity.TableInserimenti = null;
		}
	}
}
#pragma warning restore 1591
