# LR-parser

# What is it?

LR parser is a type of bottom-up parsers that efficiently handle deterministic context-free languages in guaranteed linear time.
The name LR is an acronym. The L means that the parser reads input text in one direction without backing up; that direction is typically Left to right within each line, and top to bottom across the lines of the full input file. (This is true for most parsers.) The R means that the parser produces a reversed Rightmost derivation; it does a bottom-up parse, not a top-down LL parse or ad-hoc parse. The name LR is often followed by a numeric qualifier, as in LR(1) or sometimes LR(k)
This project is the implementation of a LR(1)

# Reference Book

Compilers: Principles, Techniques, and Tools
4.7.2 Constructing LR(l) Sets of Items

Example 4.54

    S' -> S
    S -> C C
    C -> c C | d




