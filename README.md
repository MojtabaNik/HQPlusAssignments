# HQAssignments

[![Build Status](https://github.com/mojtabanik/HQPlusAssignments/actions/workflows/main.yml/badge.svg)](https://github.com/MojtabaNik/HQPlusAssignments/actions)

## Tech and packages

* [Asp.net Core](https://github.com/dotnet/aspnetcore)
* [.NET Core 5.0](https://dotnet.microsoft.com/download/dotnet/5.0)
* [Hangfire](https://www.hangfire.io/) -> .NET library to perform background processing
* [HtmlAgilityPack](https://html-agility-pack.net/) -> Parsing Html
* [EPPlus](https://epplussoftware.com/) -> Excel spreadsheets for .NET
* [MailKit](http://www.mimekit.net/) -> .NET library for IMAP, POP3, and SMTP
* [Newtonsoft.Json](https://www.newtonsoft.com/json) -> JSON framework for .NET
* [NUnit](https://nunit.org/) -> Unit-Testing framework
* [Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) -> Swagger tools for documenting API's built on ASP.NET Core

## Installation

HQAssignments requires [.NET Core](https://dotnet.microsoft.com/download/dotnet/5.0) v5.0+ to run.

Install the dependencies and start the server.

```sh
 cd .\src\Presentation\HQPlusAssignments.Services.Api\
dotnet restore
dotnet build
dotnet run
```

For Tests...

```sh
 cd .\test\HQPlusAssignments.Application.Test\
 dotnet test
 
 cd .\test\HQPlusAssignments.Common.Test\
 dotnet test
```

## Tasks

* Web Extraction
* Reporting
* Web Service / API

### Task 1: Web Extraction

> Write a data extractor that reads out content from extraction.booking.html and extracts the below listed
> information.

Here is what I did for this task:

#### 1.Created an Html extractor that reads a config file and extracts values from Html content based on it.

Sample of HtmlExtractor Config:

```JSON
{
    "Name": "RoomCategories",
    "XPath": "//*[@id=\"maxotel_rooms\"]/tbody/tr",
    "ShouldCheckInValidation": true,
    "ShouldShowInOutPut": true,
    "Childs": [
      {
        "Name": "Max",
        "XPath": "./td[1]/span/@title",
        "ShouldCheckInValidation": false,
        "ShouldShowInOutPut": true
      },
      {
        "Name": "Name",
        "XPath": "./td[2]",
        "ShouldCheckInValidation": false,
        "ShouldShowInOutPut": true
      }
    ]
  }
```

Here is JSON config description:

| Variables | Description |
| ------ | ------ |
| Name | Name of output C# object property that we want to send as JSON to an endpoint API |
| XPath | Xml path of HTML tag that we want to extract value from it |
| Type | Type of the value that we want to extract. Could be "float"  or "int" at this time |
| ShouldCheckInValidation | If it's true then our input HTML content should contain this HTML tag or it would throw an error |
| ShouldShowInOutPut | If we set this to true then our output JSON object will contain this property. we set this to false when we only want to validate this HTML tag and not to show it in our output JSON data.  |
| Childs | It's an object array of our JSON config object, with this we can get child tags recursively. When we have multiple objects like RoomCategories or AlternativeHotels we use this child array to show a list of data in our output JSON data. |

With the help of this htmlExtractor, we can retrieve data from a web page only by changing the config file. As we all know, the HTML content of a website could change over time, If we read the data statically, we would face exceptions in future. With the proposed method we only need to change our config file when a website changes its dom.

### 2.Extract hotel informations from extraction.booking.html

We can extract desire information in 3 easy steps because of the above method:

* Read HTML content from extraction.booking.html file
* Validate it
* Extract values using BookingComHtmlNodes.json config file

### 3.Create Api for the result

We can check the result from the API endpoint with the URL of "[GET] API/HotelExtractor".

### Task 2: Reporting

> Generate an Excel report based on raw data and suggest an architecture on how to automate the process that an email is sent at time x attaching the
> report.

Here is what I did for this task:

### 1.Create a method to convert any IEnumerable objects to excel with the help of the EPPlus package

You can see the code inside HQPlusAssignments.Common/Extensions/IEnumerableExtensions.cs file. This code is written as an extension method so we can easily call it on every IEnumerable object.

Code Sample

```Csharp
    var hotelList = new List<Hotel>();
    //Add some data to the list
    var fileBytes = hotelList.ToExcel();
```

### 2.Create a method to read raw data and returns excel file

There is a method named GenerateExcelFromJsonFile() inside HotelReportService, In this method, I read data from the file and convert it to C# IEnumerable list and return the ToExcel() result.

### 3.Send email

I wrote a MailService that contains the codes we need to send an email with some attachments by SMTP protocol.

### 4.Send excel report to a desire email address

With the help of MailService, we can now send our generated excel report to an email address. The code exists in HotelReportService.cs and the method name is "SendReportByEmailAsync".

### 5.Send excel report at a specific time

For this purpose I config the Hangfire package, Which is a .net Background processing package. You can find the code under HotelReportService.cs and the method name is "SendScheduleReport". You can use this URL "API/Report/SendReportByEmail" and give it your time X and your desire email address as input then get an excel attachment at the exact time in your inbox.

SMTP Config:

```json
  "MailSettings": {
    "Mail": "<FromEmail>",
    "DisplayName": "<DisplayName>",
    "Password": "<YourPassword>",
    "Host": "smt.google.com",
    "Port": 587
  }
```
To receive an email, You have to set the information above inside the appsettings.json file.

### Task 3: Web Service / API
> Write a RESTful web service including the parameters HotelID and ArrivalDate. The web service GET request
> imports the file (hotelsrates.json), filters the list by means of the parameters and returns a filtered list.

For this task, I write a method named GetHotel inside HotelService whose duty is to read data from the hotelRatesList.json file and convert it to C# List, Then Filter it based on HotelId and target date time.
