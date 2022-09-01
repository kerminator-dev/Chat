# Chat
 Full stack chat application

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
