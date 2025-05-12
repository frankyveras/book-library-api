import React, { useState } from "react";
import {
    TextField,
    Button,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableRow,
    Container,
    Typography,
    Stack,
    FormControl,
    InputLabel,
    Select,
    MenuItem,
} from "@mui/material";
import axios from "axios";

const API_BASE = "http://localhost:5285/api/books"; // adjust 

const BookSearch = () => {
    const [author, setAuthor] = useState("");
    const [isbn, setIsbn] = useState("");
    const [status, setStatus] = useState("");
    const [books, setBooks] = useState([]);

    const handleSearch = async () => {
        try {
            const params = {};
            if (author.trim()) params.author = author.trim();
            if (isbn.trim()) params.isbn = isbn.trim();
            if (status.trim()) params.status = status.trim();

            const response = await axios.get(`${API_BASE}/search`, { params });
            console.log("BOOKS", response?.data);
            setBooks(response.data);
        } catch (err) {
            setBooks([]);
            alert("No books found or error occurred.");
        }
    };


    return (
        <Container sx={{ mt: 5 }}>
            <Typography variant="h4" gutterBottom>
                Book Library Search
            </Typography>

            <Stack direction="row" spacing={2} mb={2}>
                <TextField
                    label="Author"
                    value={author}
                    onChange={(e) => setAuthor(e.target.value)}
                />
                <TextField
                    label="ISBN"
                    value={isbn}
                    onChange={(e) => setIsbn(e.target.value)}
                />
                <FormControl sx={{ minWidth: 160 }}>
                    <InputLabel>Status</InputLabel>
                    <Select
                        value={status}
                        label="Status"
                        onChange={(e) => setStatus(e.target.value)}
                    >
                        <MenuItem value="">(Any)</MenuItem>
                        <MenuItem value="own">Own</MenuItem>
                        <MenuItem value="love">Love</MenuItem>
                        <MenuItem value="want">Want to Read</MenuItem>
                    </Select>
                </FormControl>
                <Button variant="contained" onClick={handleSearch}>
                    Search
                </Button>
            </Stack>

            <Table>
                <TableHead>
                    <TableRow>
                        <TableCell>Title</TableCell>
                        <TableCell>Authors</TableCell>
                        <TableCell>Category</TableCell>
                        <TableCell>ISBN</TableCell>
                        <TableCell>Available Copies</TableCell>
                        <TableCell>Type</TableCell>
                        <TableCell>Status</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {books.map((book) => (
                        <TableRow key={book.bookId}>
                            <TableCell>{book?.title}</TableCell>
                            <TableCell>{book?.firstName} {book?.lastName}</TableCell>
                            <TableCell>{book?.category}</TableCell>
                            <TableCell>{book?.isbn}</TableCell>
                            <TableCell>{book?.copiesInUse}/{book?.totalCopies}</TableCell>
                            <TableCell>{book?.type}</TableCell>
                            <TableCell>{book?.status}</TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </Container>
    );
};

export default BookSearch;
