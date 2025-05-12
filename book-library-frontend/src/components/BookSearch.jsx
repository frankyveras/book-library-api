import React, { useState, useEffect, useCallback } from "react";
import {
    TextField,
    Button,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TablePagination,
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

const API_BASE = process.env.REACT_APP_API_BASE;

const BookSearch = () => {
    const [author, setAuthor] = useState("");
    const [isbn, setIsbn] = useState("");
    const [status, setStatus] = useState("");
    const [books, setBooks] = useState([]);
    const [noResults, setNoResults] = useState(false);
    const [page, setPage] = useState(0);
    const [rowsPerPage, setRowsPerPage] = useState(5);
    const [totalCount, setTotalCount] = useState(0);

    const fetchBooks = useCallback(async (currentPage = page, size = rowsPerPage) => {
        try {
            const response = await axios.get(`${API_BASE}`, {
                params: {
                    page: currentPage + 1,
                    pageSize: size,
                },
            });

            setBooks(response.data.items || []);
            setTotalCount(response.data?.totalItems || 0);
            setNoResults(response?.data?.items?.length === 0);
        } catch (err) {
            console.error("Error fetching books:", err);
            setNoResults(true);
        }
    }, [page, rowsPerPage]);


    useEffect(() => {
        if (!author && !isbn && !status) {
            fetchBooks();
        }
    }, [author, fetchBooks, isbn, status]);



    const handleChangePage = (event, newPage) => {
        setPage(newPage);
        if (author || isbn || status) {
            handleSearch(newPage, rowsPerPage);
        } else {
            fetchBooks(newPage, rowsPerPage);
        }
    };

    const handleChangeRowsPerPage = (event) => {
        const newSize = parseInt(event.target.value, 10);
        setRowsPerPage(newSize);
        setPage(0);
        if (author || isbn || status) {
            handleSearch(0, newSize);
        } else {
            fetchBooks(0, newSize);
        }
    };

    const handleSearch = async (currentPage = page, size = rowsPerPage) => {
        try {
            const params = {};
            if (author.trim()) params.author = author.trim();
            if (isbn.trim()) params.isbn = isbn.trim();
            if (status.trim()) params.status = status.trim();

            if (Object.keys(params).length > 0) {
                params.page = currentPage + 1;
                params.pageSize = size;
                const response = await axios.get(`${API_BASE}/search`, { params });
                if (response?.data?.items?.length === 0) {
                    setBooks([]);
                    setTotalCount(0);
                    setNoResults(true);
                } else {
                    setBooks(response.data.items || []);
                    setTotalCount(response.data?.totalItems || 0);
                }
            }

        } catch (err) {
            setBooks([]);
            setNoResults(true);
            setTotalCount(0);
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
                <Button variant="contained" onClick={() => handleSearch(page, rowsPerPage)}>
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
                    {noResults && (
                        <TableRow>
                            <TableCell colSpan={7} align="center">
                                <Typography variant="h6">No results found</Typography>
                            </TableCell>
                        </TableRow>
                    )}
                </TableBody>
            </Table>
            <TablePagination
                rowsPerPageOptions={[5, 10, 25]}
                component="div"
                count={totalCount}
                rowsPerPage={rowsPerPage}
                page={page}
                onPageChange={handleChangePage}
                onRowsPerPageChange={handleChangeRowsPerPage}
            />
        </Container>
    );
};

export default BookSearch;
