import axios from "axios";

export default class UserService {
  createUser(values) {
    const { email, password ,username, } = values;

    return axios.post(
      "http://localhost:2255/api/Auth/signup",

      email,
      password,
      username,

      {
        headers: {
          'Content-Type': 'application/json',
        },
      },
      { mode: "cors" }
    );
  }
}
  