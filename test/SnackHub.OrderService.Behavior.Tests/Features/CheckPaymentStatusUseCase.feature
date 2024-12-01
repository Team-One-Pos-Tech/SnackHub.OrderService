Feature: Check Payment Status Use Case

    Scenario: Valid order, confirm should succeed, payment status should be confirm
        Given a valid client with id '919806ea-f94d-401b-a1f6-9a15291c4125' and name 'John Doe'
        And a table of products with
          | Id                                   | Name      | Price | Description          |
          | 919806ea-f94d-401b-a1f6-9a15291c4122 | X-Tudo    | 10.5  | Completo sem abacaxi |
          | fea2f300-f37a-438f-9f10-f514ecd030b2 | Coca-cola | 9.45  | Coca gelada          |
        When confirming the order with client id '919806ea-f94d-401b-a1f6-9a15291c4125' and product details table:
          | ProductId                            | Quantity |
          | 919806ea-f94d-401b-a1f6-9a15291c4122 | 2        |
          | fea2f300-f37a-438f-9f10-f514ecd030b2 | 1        |
        Then the order status should be 'Confirmed'
        And the payment status should be 'Confirmed'