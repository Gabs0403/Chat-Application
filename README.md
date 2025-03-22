# Chat Application

A simple chat application developed using Windows Forms and SQL Server. This project allows users to register, log in, and send messages to other users. It features real-time conversations and stores data in a SQL Server database, ensuring that user information and messages are saved and accessible.

## Features

- **User Authentication**: Secure login and registration functionalities.
- **User List**: Users can view a list of other registered users.
- **Real-Time Messaging**: Send and receive messages in a conversation.
- **Database Integration**: Conversations and messages are stored in a SQL Server database.

## Technologies Used

- **C#**: The core programming language for the Windows Forms application.
- **Windows Forms**: UI framework for building the desktop application.
- **SQL Server**: Database management system for storing user information, conversations, and messages.
  
## Installation

1. Clone the repository:
    ```bash
    git clone https://github.com/yourusername/chat-application.git
    ```

2. Open the project in Visual Studio.

3. **Set up SQL Server**:
   - Create a database named `ChatAppDB`.
   - Create tables for `User`, `Conversation`, and `Message` in your SQL Server database.
   - Update the connection string in the project to match your local or cloud SQL Server setup.

4. **Build and Run**:
   - Build the project in Visual Studio.
   - Run the application, and the login screen should appear.

## Usage

1. **Login/Register**:  
   - If you're a new user, click "Create Account" to register. If you're already registered, log in with your credentials.

2. **Start a Conversation**:  
   - After logging in, view a list of other registered users. Click on a user to start a conversation.
   
3. **Send Messages**:  
   - Type messages in the text box and press "Send" to send them. The conversation history will be saved and displayed.


