namespace Surgicalogic.Model.Enum
{
    public enum ActionType
    {
        Create = 1,
        Read = 2,
        Update = 3,
        Delete = 4
    }

    public enum InfoType
    {
        Error = 1,
        Warning = 2,
        Info = 3
    }

    public enum MessageType
    {
        EquipmentRelatedToOperatingRoom = 1,
        ModelHasRelationalData = 2,
        UserNotFound = 3,
        InvalidCode = 4,
        PasswordsDoNotMatch = 5,
        AppointmentIsNotAvailable = 6,
        IsEventNumberDifferent = 7,
        CodeIsNotUnique = 8
    }

    public enum SettingKey
    {
        OperationPeriodInMinutes,
        OperationWorkingHourStart,
        OperationWorkingHourEnd,
        ClinicPeriodInMinutes,
        ClinicWorkingHourStart,
        ClinicWorkingHourEnd,
        ClinicPersonPerPeriod,
        ClinicAppointmentDays,
        SumPatientNumber,
        ForAverageTimeEveryPatient,
        ForStandardDeviationAverageTime,
        PatientsFailureRate,
        WithoutAppointmentPatientRate,
        DoctorTimePatientTimeRate,
        RoundingIntervalValue,
        SimulationIterationCount
    }

    public enum SettingDataTypeNames
    {
        String = 1,
        Int = 2,
        Time = 3,
        Double = 7
    }
}