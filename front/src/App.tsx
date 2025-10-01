import "./App.css";
import { Route, Routes } from "react-router";
import DefaultLayout from "./components/layout/DefaultLayout";
import MainPage from "./pages/mainPage/MainPage";
import LoginPage from "./pages/loginPage/LoginPage";
import { useEffect } from "react";
import { loginSucess } from "./store/slices/authSlice";
import { useDispatch } from "react-redux";
import GameListPage from "./pages/gamePage/GameListPage";

function App() {
    const dispatch = useDispatch();

    useEffect(() => {
        // read jwt token
        const token = localStorage.getItem("token");
        if(token) {
            dispatch(loginSucess(token));
        }
    }, []);

    return (
        <>
            <Routes>
                <Route path="/" element={<DefaultLayout />}>
                    <Route index element={<MainPage />} />
                    <Route path="/login" element={<LoginPage />} />
                    <Route path="/game" element={<GameListPage />} />
                </Route>
            </Routes>
        </>
    );
}

export default App;
