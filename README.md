📘 Project Overview:
###
ThingsBoard Scheduler Microservice
🎯 Purpose
This microservice is designed to automate the execution of scheduled commands by sending attribute updates to devices managed via ThingsBoard, an open-source IoT platform. It's particularly useful for scenarios like smart building automation, where devices need to perform actions (e.g., adjusting temperature, turning on/off) at specific times.

🏗️ Architecture
###
The project follows the Clean Architecture principles, ensuring a clear separation of concerns:

Domain Layer: Contains the core business entities and logic.

Application Layer: Handles use cases and orchestrates the application's operations.

Infrastructure Layer: Manages external concerns like database access and HTTP communication.

Presentation Layer: Exposes APIs and handles user interactions.

📁 Project Structure
###
Domain:
###
Defines the core entities such as Schedule, ScheduleDevice, and DeviceAttribute.

Persistence:
###
Implements data access using Entity Framework Core, including the DbContext and repository patterns.

Services: 
###
Contains the business logic, including the background service that processes schedules.

Palm:
###
Serves as the entry point of the application, configuring services and dependencies.

🗄️ Database Design
Schedule:

Id: Unique identifier.

ScheduledTime: The time when the schedule should be executed.

IsExecuted: Indicates if the schedule has been processed.

IsActive: Determines if the schedule is active.

ScheduleDevices: Collection of devices associated with the schedule.

ScheduleDevice:

DeviceId: Identifier of the device.

AccessToken: ThingsBoard access token for the device.

Attributes: Collection of attributes to be sent to the device.

DeviceAttribute:

Key: Name of the attribute.

Value: Value to be assigned to the attribute.
###

🔄 Workflow
###
Schedule Creation: Users define schedules specifying the execution time and associated devices with their respective attributes.

Background Service: Periodically checks for due schedules that haven't been executed.

Attribute Dispatch: Sends the defined attributes to each device via ThingsBoard's REST API using the device's access token.

Status Update: Marks the schedule as executed to prevent reprocessing.

🌐 ThingsBoard Integration
Authentication:
###
Utilizes device-specific access tokens for authentication.

API Endpoint: Sends HTTP POST requests to /api/v1/{accessToken}/attributes.

Payload: JSON object containing key-value pairs representing device attributes.
###
 What is ThingsBoard?
ThingsBoard is an open-source IoT platform for device management, data collection, processing, and visualization. It allows you to control devices by sending attribute updates, which are essentially key-value configurations that devices act upon.

Your microservice is built to automate sending attribute updates to ThingsBoard-managed devices based on pre-defined schedules.

🔗 Key Integration Points
1. Device Authentication via Access Token
Instead of using the JWT token for user-level access (like admin/tenant logins), your microservice authenticates at the device level using a device access token.

Each ScheduleDevice entity holds an AccessToken, which is:

A unique token assigned to a device in ThingsBoard.

Used in the URL path when calling ThingsBoard's telemetry/attribute APIs.

http
Copy
Edit
POST http://localhost:8080/api/v1/{accessToken}/attributes
🔐 This allows the microservice to interact with the device securely without full platform-level authentication.

2. Sending Attribute Updates
When a schedule is due for execution, your service sends HTTP POST requests to the ThingsBoard API endpoint with the attributes that need to be updated on the device.

Code Reference:



var url = $"{thingsBoardBaseUrl}/api/v1/{device.AccessToken}/attributes";

var body = new Dictionary<string, object>
{
    { attribute.Key, attribute.Value }
};

var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

var response = await client.PostAsync(url, content);
This updates client-side attributes on the device.

Devices listening to attribute changes (like air conditioners, smart lights, etc.) can react to this data.

3. Attribute Structure in ThingsBoard
In ThingsBoard, attributes are divided into three types:

Server-side: Set by administrators or APIs with platform credentials.

Client-side: Sent from the device.

Shared: Updated by the platform and readable by the device.

Your service is updating client-side attributes via the /api/v1/{accessToken}/attributes endpoint, which is suitable for lightweight control like toggling switches, setting temperatures, etc.

4. Real-Life Flow Example
Imagine you want to turn on a smart air conditioner at 2 PM:

A Schedule is created with ScheduledTime = Today at 2 PM.

A ScheduleDevice includes the AC's DeviceId and AccessToken.

That device has an attribute like:

json

Edit
{
  "power": "on",
  "temperature": 22
}
At 2 PM, the microservice sends the above attributes to the ThingsBoard device API.

The device reads the attributes and applies them accordingly.

5. ThingsBoard Dashboard
Once attributes are updated, ThingsBoard can:

Display them on dashboards.

Trigger Rule Engine logic (e.g., notifications, alarms).

Send downlinks to physical devices (via MQTT/CoAP/HTTP integration).

🧪 ThingsBoard API Reference (Used)
POST /api/v1/{deviceAccessToken}/attributes
Used to update device attributes.
