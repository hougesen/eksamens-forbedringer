declare interface Location {
    locationId?: number;
    locationAddress?: string;
    locationPostalCode?: string;
    cityId?: number;
    cityName?: string;
    countryId?: number;
    countryName?: string;
}

declare interface DriversAvailable {
    driversAvailableId: number;
    driversAvailableDate: Date;
    userId: number;
}

declare interface DriverInformation {
    driverInformationId: number;
    userId: number;
    locationId?: number;
    driverLicenceId?: number;
    lorryLicenceId?: number;
    eucertificate?: number;
}

// eslint-disable-next-line no-shadow
declare enum UserType {
    Admin = 1,
    Driver = 2,
}

declare interface User {
    userId: number;
    userTypeId: UserType;
    userEmail?: string;
    userFullName: string;
    userPhoneNumber?: string;
    driverInformation?: DriverInformation;
    driversAvailables?: DriversAvailable[];
}

declare interface Department {
    departmentId: number;
    departmentName: string;
    departmentContactNumber?: string;
    departmentEmail?: string;
}

// eslint-disable-next-line no-shadow
declare enum RouteStatus {
    MissingDriver = 1,
    Pending = 2,
    Completed = 3,
}

declare interface SignUpDriver {
    userId: number;
    routeId: number;
}

declare interface Route {
    routeId: ?number;
    routeDescription: ?string;
    routeStartDate: ?(Date | string);
    routeEndDate: ?(Date | string);
    routeStartLocationId: ?number;
    routeEndLocationId: ?number;
    routeHighPriority: ?boolean;
    routeStatusId: ?RouteStatus;
    userId: ?number;
    departmentId: ?number;
    routeEstTime: ?number;
    signUpDrivers: ?SignUpDriver[];
}
