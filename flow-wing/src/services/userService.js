
import axios from "axios";
 
export default class UserService {
  createUser(values) {
    const { email, username, password } = values;
 
    // Convert values to strings if necessary
    const userData = {
      Email: String(email),
      Password: String(password),
      Username: String(username)      
    };
 
    return axios.post("http://localhost:2255/api/Auth/signup", userData, {
      headers: {
        "Content-Type": "application/json",
      },
      mode: "cors",
    });
  }
}
 