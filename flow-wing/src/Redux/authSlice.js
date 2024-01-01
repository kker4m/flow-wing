import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import axios from "axios";
//Login
export const loginUser = createAsyncThunk("user/login", async (values) => {
  const request = await axios.post(
    "http://localhost:2255/api/Auth/login",
    values
  );
  const response = await request.data;
  localStorage.setItem("token", JSON.stringify(response));
  return response; // action.payload contains whatever we returned here
});
// Register
export const registerUser = createAsyncThunk(
  "user/register",
  async (values) => {
    const request = await axios.post(
      "http://localhost:2255/api/Auth/signup",
      values
    );
    const response = await request.data;
    // You may handle the registration success or store data accordingly
    return response;
  }
);

const authSlice = createSlice({
  name: "user",
  initialState: {
    user: null,
    loading: false,
    error: null,
  },
  reducers:{
    logout: (state) => {
      state.user = null
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(loginUser.pending, (state) => {
        state.loading = true;
        state.error = null;
        state.user = null;
      })
      .addCase(loginUser.fulfilled, (state, action) => {
        state.loading = false;
        state.error = null;
        state.user = action.payload;
      })
      .addCase(loginUser.rejected, (state, action) => {
        state.loading = false;
        state.error = action.error.message; // action.error.message
        state.user = null;
        console.log(action.error.message);
        // Handle the error message according to your needs
      })
      // Register reducers
      .addCase(registerUser.pending, (state) => {
        state.loading = true;
        state.error = null;
        state.user = null;
      })
      .addCase(registerUser.fulfilled, (state, action) => {
        state.loading = false;
        state.error = null;
        state.user = action.payload;
      })
      .addCase(registerUser.rejected, (state, action) => {
        state.loading = false;
        state.error = action.error.message; // action.error.message
        state.user = null;
        console.log(action.error.message);
        // Handle the error message according to your needs
      });
  },
});

export default authSlice.reducer;
