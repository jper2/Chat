---

# Chat - Demo

# Chat Application

A real-time chat application built with **.NET 9**, **SignalR**, **MongoDB**, and **JWT Authentication**. This application supports user authentication, real-time messaging, and message management.

---

## **Features**
- **User Authentication**: Secure login using JWT tokens.
- **Real-Time Messaging**: Powered by SignalR for instant updates.
- **Message Management**: Create, retrieve, and delete messages.
- **Error Handling**: Global error handling for robust API responses.

---

## **Prerequisites**
Before running the application, ensure you have the following installed:
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Node.js (v18 or later)](https://nodejs.org/)
- [MongoDB](https://www.mongodb.com/try/download/community)

---

## **Getting Started**

### **1. Clone the Repository**
Clone the repository to your local machine:

```
git clone https://github.com/your-username/chat-app.git
cd chat-app

```

---

### **2. Backend Setup**

#### **2.1. Configure MongoDB**
1. Ensure MongoDB is running locally or in a Docker container.
2. Update the MongoDB connection string in `Chat.API/appsettings.json`:

```
"MongoDb": {
  "ConnectionString": "mongodb://localhost:27017",
  "DatabaseName": "ChatApp"
}

```

#### **2.2. Configure JWT Settings**
Update the `Jwt` section in `Chat.API/appsettings.json` with your secret key:

```
"Jwt": {
  "Key": "your-secret-key",
  "Issuer": "ChatApp",
  "Audience": "ChatAppUsers",
  "ExpiryMinutes": 60
}

```

#### **2.3. Run the Backend**
Navigate to the `Chat.API` directory and run the backend:

```
cd Chat.API
dotnet run

```
The API will be available at `https://localhost:7117` (or the port specified in `launchSettings.json`).

---

### **3. Frontend Setup**

#### **3.1. Install Dependencies**
Navigate to the frontend directory (if applicable) and install dependencies:

```
cd Chat.UI.React
npm install

```

#### **3.2. Configure API Base URL**
Update the `VITE_API_BASE_URL` in the `.env` file to point to your backend:

```
VITE_API_BASE_URL=https://localhost:7117

```

#### **3.3. Run the Frontend**
Start the React development server:

```
npm run dev

```
The frontend will be available at `http://localhost:5173`. If you are using a different URL, make sure to update the `FrontendUrl` setting in `Chat.API/appsettings.json` with the new address."

```
"FrontendUrl": "http://localhost:5173",

```

---

### **4. Test the Application**

#### **4.1. Test User Authentication**
1. Use a tool like Postman or the frontend to register a new user.
2. Log in to receive a JWT token.

#### **4.2. Test Real-Time Messaging**
1. Open the chat application in two browser tabs.
2. Send a message in one tab and verify it appears in real-time in the other tab.

#### **4.3. Test Message Deletion**
1. Delete a message using the frontend or the `DELETE /api/messages/{id}` endpoint.
2. Verify the message is removed in real-time from all connected clients.

---

## **Testing**

### **Unit Tests**
Run the unit tests using the following command:

```
cd Chat.Tests
dotnet test

```

---

## **Project Structure**
- **Chat.API**: Backend API built with .NET 9.
- **Chat.Core**: Core library containing models, services, and utilities.
- **Chat.UI**: React frontend for the chat application.
- **Chat.Tests**: Unit tests for the backend.

---

## **Project Structure**
- **Chat.API**: Backend API built with .NET 9.
- **Chat.Core**: Core library containing models, services, and utilities.
- **Chat.UI**: React frontend for the chat application.
- **Chat.Tests**: Unit tests for the backend.

---

## **Functional Description**

This chat application provides the following functionalities for users:

### **1. Real-Time Messaging**
- Users can send and receive messages in real-time using SignalR.
- Messages are updated dynamically without requiring a page refresh.

### **2. Message Types**
The application supports multiple types of messages:
- **Text Messages**: Users can send and view plain text messages.
- **Image Messages**: Users can share image URLs, which are displayed as images in the chat.
- **Chart Messages**: Users can send chart data, which is rendered as visual charts.
- **Table Messages**: Users can send markdown-formatted tables, which are displayed as structured tables.

### **3. Message Management**
- **Add Messages**: Users can add new messages to the chat.
- **Delete Messages**: Users can delete their own messages. Deleted messages are removed from the chat in real-time.

### **4. Authentication**
- Users must log in to access the chat.
- The application uses JWT-based authentication to identify users.
- Logged-in users can see their username displayed in the navigation bar.

### **5. Notifications**
The application provides toast notifications for various events:
- **Error Notifications**: Displayed when an action (e.g., deleting a message) fails.
- **Reconnection Notifications**: Displayed when the application attempts to reconnect to the chat server.

### **6. Smooth Scrolling**
- The chat automatically scrolls to the latest message when new messages are added.

### **7. Responsive Design**
- The application is designed to work seamlessly on both desktop and mobile devices.

---

## **Technical Features**
- **Frontend**: Built with React and TypeScript.
- **Real-Time Communication**: Powered by SignalR for real-time updates.
- **State Management**: Uses React's `useState` and `useEffect` hooks for managing state and side effects.
- **Authentication**: JWT-based authentication with user context (`AuthContext`).
- **Error Handling**: Provides user-friendly error messages using `react-toastify`.

---