# Review
 Full stack chat application.
 Main functionality is abilities to receive and send in real-time text messages to another users, update your own profile image, block/unblock users and have multiple devices, connected to account. 

# To do: back-end and client apps:
## Back-end methods:
Authentication:
- ✅ Registration
- ✅ Login
- ✅ Refresh token

Dialogues:
- ✅ Create dialogue, real-time SignalR notficiation 
- ✅ Delete dialogue, real-time SignalR notficiation 
- ✅ Get user dialogues list

Messages:
- ✅ Send message to dialogue, real-time SignalR notficiation 
- ✅ Delete messages in dialogue, real-time SignalR notficiation 
- ✅ Update message in dialogue, real-time SignalR notficiation 
- ✅ Get user's message list 

Users:
- ✅ Find user's by username
- ✅ Get user's info by user id
- ❌ Add user to black list
- ❌ Delete user from black list

Profile:
- ❌ Update avatar (256x256 - big size, also sever side has small size 32x32 and blur hash string)
- ❌ Update name
- ✅ Update password

Tiny notifications:
- ❌ User gets online/offline
- ❌ User typing message in dialogue

Conversations (Optional & not necessary):
- ❌ Create conversation with multiple users
- ❌ Join conversation
- ❌ Left conversation

## Client side:
- ❌ MVVM .NET MAUI Android app
- ❌ MVVM .NET MAUI Windows app (optional)
- 🚩 <a href="https://github.com/ertanfird/simplify">Single page React web app</a> by <a href="https://github.com/ertanfird">Ertanfird</a>

## Refactoring:
- ❌ Review database
- ❌ Review code semantics
- ❌ Review exception handling
- ❌ Comment logic
- ❌ Try to remake projects with a <a href="Trying to remake it for Clean Acritecture">Clean Acritecture</a> template


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
