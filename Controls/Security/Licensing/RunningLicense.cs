namespace IndianaPark.Tools.Security.Licensing
{
    /// <summary>
    /// Generica licenza rilasciata ad un componente
    /// </summary>
    public class RunningLicense : System.ComponentModel.License
    {
		#region Fields 

		#region Internal Fields 

        private readonly string m_code;

		#endregion Internal Fields 

		#region Public Fields 

        /// <summary>
        /// When overridden in a derived class, gets the license key granted to this component.
        /// </summary>
        /// <value></value>
        /// <returns>A license key granted to this component.</returns>
        public override string LicenseKey
        {
            get { return this.m_code; }
        }

		#endregion Public Fields 

		#endregion Fields 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="RunningLicense"/> class.
        /// </summary>
        protected RunningLicense()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RunningLicense"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        public RunningLicense( string code )
        {
            this.m_code = code;
        }

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// When overridden in a derived class, disposes of the resources used by the license.
        /// </summary>
        public override void Dispose()
        {
            return;
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}