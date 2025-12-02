# 🚀 SpaceX Project

## 📸 UI Preview

| **Login (Light Mode)** | **Login – Mobile View (Dark Mode)** |
|:---:|:---:|
| <img width="1912" height="896" alt="image" src="https://github.com/user-attachments/assets/c11db0f6-6ce9-4e79-ba3b-5a2baa739c9c" /> | <img width="485" height="797" alt="image" src="https://github.com/user-attachments/assets/534648c6-60ab-4df8-8364-0db871683272" /> |


| **Dashboard (Light Mode)** | **Launches Table (Dark Mode)** |
|:---:|:---:|
| <img width="1667" height="811" alt="image" src="https://github.com/user-attachments/assets/a59c9d95-cdc5-4808-86c7-0f31a8d90ed7" /> | <img width="1661" height="802" alt="image" src="https://github.com/user-attachments/assets/c6f5f302-3af3-48be-9b18-87d65816ab28" /> |


| **Dashboard – Mobile View (Dark Mode)** | **Launches Table – Mobile View (Light Mode)** |
|:---:|:---:|
| <img width="485" height="756" alt="image" src="https://github.com/user-attachments/assets/9ca450a0-e129-4ad1-89cf-13220ca8aec1" /> | <img width="486" height="801" alt="image" src="https://github.com/user-attachments/assets/ebe579ac-e414-4da2-b632-9d76d04d0402" /> |


| **Rocket Details – Mobile View (Dark Mode)** | **Rocket Details (Light Mode)** |
|:---:|:---:|
| <img width="481" height="791" alt="image" src="https://github.com/user-attachments/assets/3317fbbe-b33a-431e-8442-0fe508597bc4" /> | <img width="1914" height="903" alt="image" src="https://github.com/user-attachments/assets/3c7cdb48-8363-4f32-ac79-23a3f6e9cc78" /> |


| **Out of Service (Light Mode)** | **Out of Service – Mobile View (Dark Mode)** |
|:---:|:---:|
| <img width="1916" height="891" alt="image" src="https://github.com/user-attachments/assets/8f3a7dd9-c5d0-42cf-b23f-57d64baddf96" /> | <img width="485" height="791" alt="image" src="https://github.com/user-attachments/assets/9cb53b46-e07a-4b63-8351-047ab2fcb0ad" /> |

---

## 🚀 Getting Started

Follow these steps to set up the project locally.

### 1. Prerequisites
* **.NET 10 SDK** (or compatible preview)
* **Node.js** (Latest LTS)
* **SQL Server** (LocalDB or Docker)

### 2. Backend Setup

1.  **Clone the repository:**
    ```bash
    git clone [https://github.com/Marce35/SpaceXProject.git](https://github.com/Marce35/SpaceXProject.git)
    cd SpaceXProject/SpaceXProject.api
    ```

2.  **Configure User Secrets:**
    This project requires encryption keys to run. Initialize and set your secrets:
    ```bash
    dotnet user-secrets init
    ```
    
    Add the following configuration (replace placeholders with real Base64 strings):
    ```json
    {
      "IdentityEncryptionKeys:Keys:0": "YOUR_AES_ENCRYPTION_KEY_BASE64==",
      "IdentityEncryptionKeys:Keys:1": "YOUR_OTHER_AES_ENCRYPTION_KEY_BASE64==",
      "IdentityEncryptionKeys:CurrentIdentityKeyId": "0",
      "JwtSettings:Key": "YOUR_OWN_JWT_KEY="
    }
    ```

3.  **Publish the Database:**
    You must deploy the database schema before running the app. Locate the **`SpaceXDb.publish.xml`** file in the Database project folder and run the publish profile (via Visual Studio or CLI) to create/update your SQL Server instance.

4.  **Run the API:**
    ```bash
    dotnet run
    ```
    The API will start at `https://localhost:7200`.

### 3. Frontend Setup

1.  **Navigate to the Client folder:**
    ```bash
    cd ../SpaceXProject.client
    ```

2.  **Install Dependencies:**
    ```bash
    npm install
    ```

3.  **Run the Application:**
    ```bash
    ng serve
    ```

4.  Open your browser at `https://localhost:4200`.
