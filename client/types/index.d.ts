declare interface Location {
    LocationId?: number;
    LocationAddress?: string;
    LocationPostalCode?: string;
    CityId?: number;
    CityName?: string;
    CountryId?: number;
    CountryName?: string;
}

declare interface DriversAvailable {
    DriversAvailableId: number;
    DriversAvailableDate: Date;
}

declare interface DriverInformation {
    DriverInformationId: number;
    UserId: number;
    LocationId?: number;
    DriverLicenceId?: number;
    LorryLicenceId?: number;
    Eucertificate?: number;
}

// eslint-disable-next-line no-shadow
declare enum UserType {
    Driver = 0,
    Admin = 1,
}

declare interface User {
    UserId: number;
    UserTypeId: UserType;
    UserEmail?: string;
    UserFullName: string;
    UserPhoneNumber?: number;
    DriverInformation?: DriverInformation;
    availableDates?: DriversAvailable[];
}

declare interface Department {
    DepartmentId: number;
    DepartmentName: string;
    DepartmentContactNumber?: string;
    DepartmentEmail?: string;
}

// eslint-disable-next-line no-shadow
declare enum RouteStatus {
    MissingDriver = 0,
    Pending = 1,
    Completed = 2,
}

declare interface Route {
    RouteId?: number;
    RouteDescription?: string;
    RouteStartDate?: Date;
    RouteEndDate?: Date;
    RouteStartLocationId?: number;
    RouteEndLocationId?: number;
    RouteHighPriority?: boolean;
    RouteStatusId?: RouteStatus;
    UserId?: number;
    DepartmentId?: number;
    RouteEstTime?: number;
}
