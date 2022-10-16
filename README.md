# Review
 ## Full stack chat application.
 
This project was made to improve knowlegde of building a .NET WEB applications with WEB Api and also can have many flaws. I would be appreciate for a new suggestions to improve the code. 
 
 ### Main functionality:
 - Register/Login/Logout
 - Receive and send in real-time text messages from/to another users throw dialogues
 - Update profile image and profile data
 - Block/unblock users 
 - Have multiple devices with different client apps, connected to one account. 
 
### Current back-end stack:
- ASP .NET 6 Web Api
- Json Web Tokens
- Entity Framework
- SQLite
- SignalR
- BCrypt

# 🚩 To do: back-end and client apps:
## Back-end methods:
✅ Authentication:
- ✅ Registration
- ✅ Login
- ✅ Refresh token

✅ Dialogues:
- ✅ Create dialogue, real-time SignalR notficiation 
- ✅ Delete dialogue, real-time SignalR notficiation 
- ✅ Get user dialogues list

✅ Messages:
- ✅ Send message to dialogue, real-time SignalR notficiation 
- ✅ Delete messages in dialogue, real-time SignalR notficiation 
- ✅ Update message in dialogue, real-time SignalR notficiation 
- ✅ Get user's message list 

✅ Users:
- ✅ Find user's by username
- ✅ Get user's info by user id

❌ Profile:
- ❌ Update user color
- ❌ Update name
- ✅ Update password

❓ Tiny notifications (optional and not necessary):
- ❓ User gets online/offline
- ❓ User typing message in dialogue

## Client side:
- ❌ MVVM .NET WPF client app
- 🚩 <a href="https://github.com/ertanfird/simplify">Single page React web app</a> by <a href="https://github.com/ertanfird">Ertanfird</a>

## Refactoring:
- ❌ Review database models
- ❌ Review code semantics
- ❌ Review exception handling
- ❌ Comment logic
- ❌ Try to remake projects with a <a href="Trying to remake it for Clean Acritecture">Clean Acritecture</a> template

## Testing


# API-methods:

### Authentication.Register
<table>
  <thead align="center">
    <tr border: none;>
      <td>Option</td>
      <td>Value</td>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>Description</td>
      <td>Register user</td>
    </tr>
    <tr>
      <td>API-method</td>
      <td>/api/Authentication/Register</td>
    </tr>
    <tr>
      <td>HTTP-method</td>
      <td>POST</td>
    </tr>
    <tr>
      <td>Token</td>
      <td></td>
    </tr>
  </tbody>
</table>


Request body:
```js
{
  "username": "string",
  "password": "string",
  "name": "string"
}
```
---
### Authentication.Login
<table>
  <thead align="center">
    <tr border: none;>
      <td>Option</td>
      <td>Value</td>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>Description</td>
      <td>Sign in existing account</td>
    </tr>
    <tr>
      <td>API-method</td>
      <td>/api/Authentication/Login</td>
    </tr>
    <tr>
      <td>HTTP-method</td>
      <td>POST</td>
    </tr>
    <tr>
      <td>Token</td>
      <td></td>
    </tr>
  </tbody>
</table>

Request body:
```js
{
  "username": "string",
  "password": "string"
}
```
---
### Authentication.RefreshToken
<table>
  <thead align="center">
    <tr border: none;>
      <td>Option</td>
      <td>Value</td>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>Description</td>
      <td>Refresh access-token & refresh-token</td>
    </tr>
    <tr>
      <td>API-method</td>
      <td>/api/Authentication/RefreshToken</td>
    </tr>
    <tr>
      <td>HTTP-method</td>
      <td>POST</td>
    </tr>
    <tr>
      <td>Token</td>
      <td></td>
    </tr>
  </tbody>
</table>

Request body:
```js
{
  "refreshToken": "string"
}
```
---
### Dialogues.All
<table>
  <thead align="center">
    <tr border: none;>
      <td>Option</td>
      <td>Value</td>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>Description</td>
      <td>Get all user's dialogues</td>
    </tr>
    <tr>
      <td>API-method</td>
      <td>/api/Dialogues/All</td>
    </tr>
    <tr>
      <td>HTTP-method</td>
      <td>GET</td>
    </tr>
    <tr>
      <td>Token</td>
      <td>Required access token</td>
    </tr>
  </tbody>
</table>

Request body empty

---
