namespace Employment.Web.Mvc.Infrastructure.Types
{
    /// <summary>
    /// Represents an enum which defines the area link icon type.
    /// </summary>
    public enum AreaLinkIconType
    {
        /// <summary>
        /// No icon.
        /// </summary>
        None,

        #region Activity

        /// <summary>
        /// Add Activity icon for Activity area.
        /// </summary>
        ActivityAdd,

        /// <summary>
        /// Add Activity Host icon for Activity area.
        /// </summary>
        ActivityAddHost,

        /// <summary>
        /// Add Activity Placement icon for Activity area.
        /// </summary>
        ActivityAddPlacement,

        /// <summary>
        /// Search Activity icon for Activity area.
        /// </summary>
        ActivitySearch,

        /// <summary>
        /// Search Activity Host icon for Activity area.
        /// </summary>
        ActivitySearchHost,

        /// <summary>
        /// Search Activity Placement icon for Activity area.
        /// </summary>
        ActivitySearchPlacement,

        /// <summary>
        /// Search Activity Diary icon for Activity area.
        /// </summary>
        ActivitySearchDiary,

        #endregion

        #region Participation Account

        /// <summary>
        /// Add Expenditure icon for Participation Account area.
        /// </summary>
        ParticipationAccountAddExpenditure,

        /// <summary>
        /// Search Expenditure icon for Participation Account area.
        /// </summary>
        ParticipationAccountSearchExpenditure,

        /// <summary>
        /// Adjustment Request icon for Participation Account area.
        /// </summary>
        ParticipationAccountAdjustmentRequest,

        /// <summary>
        /// Data Recovery icon for Participation Account area.
        /// </summary>
        ParticipationAccountDataRecovery,

        /// <summary>
        /// Partial Recovery icon for Participation Account area.
        /// </summary>
        ParticipationAccountPartialRecovery,

        /// <summary>
        /// Job Seeker History icon for Participation Account area.
        /// </summary>
        ParticipationAccountJobSeekerHistory,

        /// <summary>
        /// Request History icon for Participation Account area.
        /// </summary>
        ParticipationAccountRequestHistory,

        /// <summary>
        /// Request Transfer icon for Participation Account area.
        /// </summary>
        ParticipationAccountRequestTransfer,

        /// <summary>
        /// Maintain Credits icon for Participation Account area.
        /// </summary>
        ParticipationAccountMaintainCredits,

        /// <summary>
        /// View Summary icon for Participation Account area.
        /// </summary>
        ParticipationAccountViewSummary,

        #endregion

        #region Payments

        /// <summary>
        /// Add General icon for Payments area.
        /// </summary>
        PaymentsAddGeneral,

        /// <summary>
        /// Add Override icon for Payments area.
        /// </summary>
        PaymentsAddOverride,

        /// <summary>
        /// Job Seeker History icon for Payments area.
        /// </summary>
        PaymentsJobSeekerHistory,

        /// <summary>
        /// Search Claimed icon for Payments area.
        /// </summary>
        PaymentsSearchClaimed,

        /// <summary>
        /// Search Earning Reduction icon for Payments area.
        /// </summary>
        PaymentsSearchEarningReduction,

        /// <summary>
        /// Search Available icon for Payments area.
        /// </summary>
        PaymentsSearchAvailable,

        #endregion

        #region ServicesAdministration

        /// <summary>
        /// Search Contract icon for Services Administration area.
        /// </summary>
        ServicesAdministrationSearchContract,

        /// <summary>
        /// Search Outlet icon for Services Administration area.
        /// </summary>
        ServicesAdministrationSearchOutlet,

        /// <summary>
        /// Search Site icon for Services Administration area.
        /// </summary>
        ServicesAdministrationSearchSite,

        #endregion

        #region Employer

        /// <summary>
        /// Add Employer icon for Employer area.
        /// </summary>
        EmployerAddEmployer,

        /// <summary>
        /// Add Vacancy icon for Employer area.
        /// </summary>
        EmployerAddVacancy,

        /// <summary>
        /// Add Placement icon for Employer area.
        /// </summary>
        EmployerAddPlacement,

        /// <summary>
        /// Search Employer icon for Employer area.
        /// </summary>
        EmployerSearchEmployer,

        /// <summary>
        /// Search Vacancy icon for Employer area.
        /// </summary>
        EmployerSearchVacancy,

        /// <summary>
        /// Search Placement icon for Employer area.
        /// </summary>
        EmployerSearchPlacement,

        /// <summary>
        /// Find staff icon for Employer area.
        /// </summary>
        EmployerFindStaff,

        #endregion

        /// <summary>
        /// The default area link icon is none.
        /// </summary>
        Default = None
    }
}
