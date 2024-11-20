BLOOD BANK MANAGEMENT SYSTEM USING REST API

This project is a RESTful API for managing a Blood Bank system using ASP.NET Core. It provides CRUD operations, pagination, search, and sorting functionalities. Data is stored in an in-memory list (List<blood>), and validations ensure data integrity. The API can be tested using tools like Swagger and Postman.

FEATURES

CRUD Operations

Create, Read, Update, and Delete blood bank entries.

Pagination

Retrieve data page by page for large datasets.

Search Functionality

Search by blood type, status, or donor name.

Sorting

Sort entries by blood type or collection date.

Filtering

Filter results using multiple search parameters

ENDPOINTS 

CRUD Endpoints

POST /api/blood

Adds a new blood entry after validating inputs.

GET /api/blood

Retrieves all blood bank data.

GET /api/blood/id?id={id}

Retrieves a single blood entry by its unique ID.

PUT /api/blood/id?id={id}

Updates an existing blood entry by ID.

DELETE /api/blood?id={id}

Deletes a blood entry by ID.

PAGINATION

GET /api/blood/getPageData?page={pageNumber}&size={pageSize}

Returns paginated results based on page number and page size.

SEARCH

GET /api/blood/sortByDate

Sorts entries by collection date.

GET /api/blood/sortByBlood

Sorts entries by blood type.

FILTERING

GET /api/blood/GetFilterdData?BloodType={bloodType}&status={status}&donorName={donorName}

Filters data by one or more criteria.

DATA MODEL

## Data Model

The `blood` model has the following attributes:

| Attribute        | Type       | Description                                |
|------------------|------------|--------------------------------------------|
| `Id`             | `int`      | Unique identifier for each entry.         |
| `DonorName`      | `string`   | Name of the blood donor.                  |
| `Age`            | `int`      | Age of the donor.                         |
| `BloodType`      | `string`   | Blood group (e.g., A+, O-).               |
| `ContactInfo`    | `string`   | Donor's contact details (10-digit).       |
| `Quantity`       | `int`      | Quantity of blood donated (in ml).        |
| `CollectionDate` | `DateTime` | Date of blood collection.                 |
| `ExpiratoinDate` | `DateTime` | Expiration date of the blood unit.        |
| `Status`         | `string`   | Status of the blood entry (e.g., Available, Requested, Expired). |

